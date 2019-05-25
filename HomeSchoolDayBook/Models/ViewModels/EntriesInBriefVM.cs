using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class EntriesInBriefVM
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Title { get; set; }

        [Display(Name = "Students")]
        public List<string> StudentNames { get; set; }

        [Display(Name = "Subjects")]
        public List<string> SubjectNames { get; set; }

        public EntriesInBriefVM(DateTime date, string title, List<string> studentNames, List<string> subjectNames)
        {
            Date = date;
            Title = title;
            StudentNames = studentNames;
            SubjectNames = subjectNames;
        }
    }
}
