using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Data;
using Newtonsoft.Json;


namespace HomeSchoolDayBook.Models.ViewModels
{
    public class EntryCreateEditVM
    {
        public Entry Entry { get; set; }

        public string NoStudentsMessage { get; set; }

        public string NoSubjectsMessage { get; set; }

        [Display(Name = "Hours")]
        [Range(0, 24, ErrorMessage ="Hours must be positive and less than 24.")]
        public double? EnteredHours { get; set; }

        [Display(Name = "Minutes")]
        [Range(0, 1440, ErrorMessage ="Minutes must be positive and less than 1440")]
        public double? EnteredMinutes { get; set; }

        public int? EnteredTotalMinutes
        {
            get
            {
                if (EnteredHours == null && EnteredMinutes == null) return null;

                return (int)(((EnteredHours ?? 0) * 60) + (EnteredMinutes ?? 0));
            }
        }

        [Display(Name = "Subjects")]
        public List<CheckBoxVM> SubjectCheckBoxes { get; set; }

        [Display(Name = "Students")]
        public List<CheckBoxVM> StudentCheckBoxes { get; set; }

        public string GradesJSON { get; set; }

        //constructor for Create OnGet
        public EntryCreateEditVM (ApplicationDbContext context, string userId )
        {
            Entry = new Entry
            {
                Date = DateTime.Today,
                UserID = userId
            };

            SubjectCheckBoxes = context
                .Subjects
                .Where(su => su.UserID == userId)
                .Where(su => su.IsActive)
                .OrderBy(su => su.Name)
                .Select(su => new CheckBoxVM(su.ID, su.Name, false ))
                .ToList();

            if (SubjectCheckBoxes.Count == 0)
            {
                NoSubjectsMessage = "You have no saved Subjects.";
            }

            StudentCheckBoxes = context
                .Students
                .Where(st => st.UserID == userId)
                .Where(st => st.IsActive)
                .OrderBy(st => st.Name)
                .Select(st => new CheckBoxVM(st.ID, st.Name, false))
                .ToList();

            if (StudentCheckBoxes.Count == 0)
            {
                NoStudentsMessage = "You have no saved Students.";
            }

            GradesJSON = "{}";
        }

        //constructor for Create OnPost, and Edit
        public EntryCreateEditVM (Entry entry, ApplicationDbContext context, string userId)
        {
            Entry = entry;
            EnteredHours = entry.ComputedHours;
            EnteredMinutes = entry.ComputedMinutes;

            HashSet<int> entrySubjectIDs = entry
                .SubjectAssignments
                .Select(sa => sa.SubjectID)
                .ToHashSet();

            SubjectCheckBoxes = context
                .Subjects
                .Where(su => su.UserID == userId)
                .Where(su => su.IsActive || entrySubjectIDs.Contains(su.ID))
                .OrderBy(su => su.Name)
                .Select(su => new CheckBoxVM(su.ID, su.Name, entrySubjectIDs.Contains(su.ID)))                
                .ToList();

            HashSet<int> entryStudentIDs = entry
                .Enrollments
                .Select(enr => enr.StudentID)
                .ToHashSet();

            StudentCheckBoxes = context
                .Students
                .Where(st => st.UserID == userId)
                .Where(st => st.IsActive || entryStudentIDs.Contains(st.ID))
                .OrderBy(st => st.Name)
                .Select(st => new CheckBoxVM(st.ID, st.Name, entryStudentIDs.Contains(st.ID)))
                .ToList();

            Dictionary<string, decimal> gradesDictionary = new Dictionary<string, decimal>();
            foreach (Grade grade in entry.Grades)
            {
                gradesDictionary.Add($"earned-student-{grade.StudentID}-subject-{grade.SubjectID}", grade.PointsEarned);
                gradesDictionary.Add($"available-student-{grade.StudentID}-subject-{grade.SubjectID}", grade.PointsAvailable);
            }

            GradesJSON = $"{JsonConvert.SerializeObject(gradesDictionary)}";
        }
    }
}
