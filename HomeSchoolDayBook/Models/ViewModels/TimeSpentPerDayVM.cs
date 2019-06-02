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
    public class TimeSpentPerDayVM
    {
        [Display(Name = "Student")]
        public string StudentName { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "Date")]
        public string FormattedDate
        {
            get
            {
                return $"{Date.ToShortDateString()} ({Date.DayOfWeek})";
            }
        }
        
        public int MinutesSpent { get; set; }

        [Display(Name = "Time Spent")]
        public string ComputedTimeSpent
        {
            get
            {
                return GetTimeSpentDisplay(MinutesSpent);
            }
        }

        public TimeSpentPerDayVM(string studentName, DateTime date, int minutesSpent)
        {
            StudentName = studentName;
            Date = date;
            MinutesSpent = minutesSpent;
        }
    }
}
