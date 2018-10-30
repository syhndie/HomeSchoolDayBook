using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;


namespace HomeSchoolDayBook.Models.ViewModels
{
    public class EntryVM
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }

        [Display(Name = "Time Spent")]
        [DataType(DataType.Text)]
        public int? EnteredHours { get; set; }

        [DataType(DataType.Text)]
        public int? EnteredMinutes { get; set; }

        public int? EnteredTotalMinutes
        {
            get
            {
                if (EnteredHours == null && EnteredMinutes == null) return null;

                return ((EnteredHours ?? 0) * 60) + (EnteredMinutes ?? 0);
            }
        }

        [Display(Name = "Subjects")]
        public List<CheckBoxVM> SubjectCheckBoxes { get; set; }

        [Display(Name = "Students")]
        public List<CheckBoxVM> StudentCheckBoxes { get; set; }

        public EntryVM (Entry entry, ApplicationDbContext context)
        {
            ID = entry.ID;
            Date = entry.Date;
            Title = entry.Title;
            Description = entry.Description;
            EnteredHours = entry.ComputedHours;
            EnteredMinutes = entry.ComputedMinutes;

            HashSet<Subject> allSubjects = context.Subjects.ToHashSet();

            HashSet<int> entrySubjectIDs = entry
                .SubjectAssignments
                .Select(sa => sa.SubjectID)
                .ToHashSet();

            SubjectCheckBoxes = allSubjects
                .Select(s => new CheckBoxVM(s.ID, s.Name, entrySubjectIDs.Contains(s.ID)))
                .ToList();

            HashSet<Student> allStudents = context.Students.ToHashSet();

            HashSet<int> entryStudentIDs = entry
                .Enrollments
                .Select(enr => enr.StudentID)
                .ToHashSet();

            StudentCheckBoxes = allStudents
                .Select(st => new CheckBoxVM(st.ID, st.Name, entryStudentIDs.Contains(st.ID)))
                .ToList();
        }
    }
}
