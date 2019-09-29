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
        [Required]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Hours must be positive.")]
        public int? Hours { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Minutes must be positive.")]
        public int? Minutes { get; set; }

        [Display(Name = "Subjects")]
        public List<CheckBoxVM> SubjectCheckBoxes { get; set; }

        [Display(Name = "Students")]
        public List<CheckBoxVM> StudentCheckBoxes { get; set; }

        public string NoSubjectsMessage
        {
            get
            {
                if (SubjectCheckBoxes != null && SubjectCheckBoxes.Count == 0) return "You have no active subjects.";
                return "";
            }
        }

        public string NoStudentsMessage
        {
            get
            {
                if (StudentCheckBoxes != null && StudentCheckBoxes.Count == 0) return "You have no active students.";
                return "";
            }
        }

        public string GradesJSON { get; set; }

        public EntryCreateEditVM()
        {

        }
        //constructor for Create OnGet
        public EntryCreateEditVM (ApplicationDbContext context, string userId )
        {
            Date = DateTime.Today;

            SubjectCheckBoxes = context
                .Subjects
                .Where(su => su.UserID == userId)
                .Where(su => su.IsActive)
                .OrderBy(su => su.Name)
                .Select(su => new CheckBoxVM(su.ID, su.Name, false ))
                .ToList();

            StudentCheckBoxes = context
                .Students
                .Where(st => st.UserID == userId)
                .Where(st => st.IsActive)
                .OrderBy(st => st.Name)
                .Select(st => new CheckBoxVM(st.ID, st.Name, false))
                .ToList();

            GradesJSON = "{}";
        }

        //constructor for Edit OnGet
        public EntryCreateEditVM (Entry entry, ApplicationDbContext context, string userId)
        {
            Title = entry.Title;
            Date = entry.Date;
            Description = entry.Description;
            Hours = entry.MinutesSpent / 60;
            Minutes = entry.MinutesSpent % 60;

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
