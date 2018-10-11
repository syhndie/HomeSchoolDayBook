using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Models
{
    public class Entry
    {
        public int ID { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int MinutesSpent { get; set; }

        public int ComputesHours
        {
            get
            {
                return MinutesSpent / 60;
            }
        }

        public int ComputedMinutes
        {
            get
            {
                return MinutesSpent % 60;
            }
        }

        [Display(Name = "Time Spent")]
        public string ComputedTimeSpent
        {
            get
            {
                string hoursUnits = ComputesHours == 1 ? "hour" : "hours";
                string minutesUnits = ComputedMinutes == 1 ? "minute" : "minutes";
                string hours = ComputesHours == 0 ? "" : $"{ComputesHours} {hoursUnits}";
                string minutes = ComputedMinutes == 0 ? "" : $"{ComputedMinutes} {minutesUnits}";
                return hours == "" || minutes == "" ? $"{hours} {minutes}" : $"{hours}, {minutes}";
            }
        }

        public string Description { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<SubjectAssignment> SubjectAssignments { get; set; }

    }
}
