using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeSchoolDayBook.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static HomeSchoolDayBook.Helpers.Helpers;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class TimeSpentPerSubjectVM
    {
        [Display(Name = "Student")]
        public string StudentName { get; set; }

        [Display(Name = "Subject")]
        public string SubjectName { get; set; }

        public int MinutesSpent { get; set; }

        [Display(Name = "Time Spent")]
        public string ComputedTimeSpent
        {
            get
            {
                return GetTimeSpentDisplay(MinutesSpent);
            }
        }
        
        public TimeSpentPerSubjectVM(string studentName, string subjectName, int minutesSpent)
        {
            StudentName = studentName;
            SubjectName = subjectName;
            MinutesSpent = minutesSpent;
        }
    }
}
