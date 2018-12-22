using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeSchoolDayBook.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class TimeSpentPerDayVM
    {
        public Student Student { get; set; }

        public Dictionary<string, string> TimePerDayLookup { get; set; }

        public TimeSpentPerDayVM(ApplicationDbContext context, int studentID, DateTime startDate, DateTime endDate)
        {
            Student = context.Students.FirstOrDefault(m => m.ID == studentID);

            var test = context.Entries
                .Include(ent => ent.Enrollments)
                .Where(ent => startDate <= ent.Date && ent.Date <= endDate)
                .Where(ent => ent.Enrollments.Select(enr => enr.StudentID).Contains(studentID))
                .GroupBy(ent => ent.Date)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.MinutesSpent));

            TimePerDayLookup = new Dictionary<string, string>();

            foreach (var day in test)
            {
                int? computedHours = day.Value / 60;
                int? computedMinutes = day.Value % 60;
                string hoursUnits = computedHours == 1 ? "hour" : "hours";
                string minutesUnits = computedMinutes == 1 ? "minute" : "minutes";
                string hours = (computedHours == 0 || computedHours == null) ? "" : $"{computedHours} {hoursUnits}";
                string minutes = (computedMinutes == 0 || computedMinutes == null) ? "" : $"{computedMinutes} {minutesUnits}";
                string timeDisplay = (hours == "" || minutes == "") ? $"{hours} {minutes}" : $"{hours}, {minutes}";

                TimePerDayLookup.Add($"{day.Key.ToShortDateString()} ({day.Key.DayOfWeek})", 
                    (timeDisplay == " " || timeDisplay == null) 
                    ? "All entries for this day have no time values entered." 
                    : timeDisplay);
            }
        }
    }
}
