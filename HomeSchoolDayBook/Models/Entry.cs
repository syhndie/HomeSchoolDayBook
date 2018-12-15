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

        [Required]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public int? MinutesSpent { get; set; }

        public int? ComputedHours
        {
            get
            {
                return MinutesSpent / 60;
            }
        }

        public int? ComputedMinutes
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
                string hoursUnits = ComputedHours == 1 ? "hour" : "hours";
                string minutesUnits = ComputedMinutes == 1 ? "minute" : "minutes";
                string hours = (ComputedHours == 0 || ComputedHours == null) ? "" : $"{ComputedHours} {hoursUnits}";
                string minutes = (ComputedMinutes == 0 || ComputedMinutes == null) ? "" : $"{ComputedMinutes} {minutesUnits}";
                return (hours == "" || minutes == "") ? $"{hours} {minutes}" : $"{hours}, {minutes}";
            }
        }

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<SubjectAssignment> SubjectAssignments { get; set; }

    }
}
