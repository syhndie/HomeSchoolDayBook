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
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }

        [Display(Name = "Time Spent")]
        [Range(0, 24, ErrorMessage ="Hours must be positive and less than 24.")]
        public double? EnteredHours { get; set; }

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

        public EntryVM()
        {

        }

        //constructor for Create page
        public EntryVM (ApplicationDbContext context)
        {
            Date = DateTime.Today;

            SubjectCheckBoxes = context
                .Subjects
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .Select(s => new CheckBoxVM(s.ID, s.Name, false ))
                .ToList();

            StudentCheckBoxes = context
                .Students
                .Where(st => st.IsActive)
                .OrderBy(st => st.Name)
                .Select(st => new CheckBoxVM(st.ID, st.Name, false))
                .ToList();
        }

        //constructor for Edit page
        public EntryVM (Entry entry, ApplicationDbContext context)
        {
            ID = entry.ID;
            Date = entry.Date;
            Title = entry.Title;
            Description = entry.Description;
            EnteredHours = entry.ComputedHours;
            EnteredMinutes = entry.ComputedMinutes;

            HashSet<int> entrySubjectIDs = entry
                .SubjectAssignments
                .Select(sa => sa.SubjectID)
                .ToHashSet();

            SubjectCheckBoxes = context
                .Subjects
                .Where(s => s.IsActive || entrySubjectIDs.Contains(s.ID))
                .OrderBy(s => s.Name)
                .Select(s => new CheckBoxVM(s.ID, s.Name, entrySubjectIDs.Contains(s.ID)))
                
                .ToList();

            HashSet<int> entryStudentIDs = entry
                .Enrollments
                .Select(enr => enr.StudentID)
                .ToHashSet();

            StudentCheckBoxes = context
                .Students
                .Where(st => st.IsActive || entryStudentIDs.Contains(st.ID))
                .OrderBy(st => st.Name)
                .Select(st => new CheckBoxVM(st.ID, st.Name, entryStudentIDs.Contains(st.ID)))
                .ToList();
        }
    }
}
