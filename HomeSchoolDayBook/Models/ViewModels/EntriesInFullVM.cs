using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class EntriesInFullVM
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Students { get; set; }

        public string Subjects { get; set; }

        public string ComputedTimeSpent { get; set; }

        public List<Grade> Grades { get; set; }

        public EntriesInFullVM(DateTime date, string title, string desciption, string students, string subjects, string computedTimeSpent, List<Grade> grades)
        {
            Date = date;
            Title = title;
            Description = desciption;
            Students = students;
            Subjects = subjects;
            ComputedTimeSpent = computedTimeSpent;
            Grades = grades;
        }
    }
}
