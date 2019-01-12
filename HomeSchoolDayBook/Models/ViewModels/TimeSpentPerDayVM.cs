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

        public Dictionary<DateTime, int?> TimePerDayLookup { get; set; }

        public TimeSpentPerDayVM(ApplicationDbContext context, int studentID, DateTime startDate, DateTime endDate, string userId)
        {
            Student = context.Students
                .Where(st => st.UserID == userId)
                .Where(st => st.ID == studentID)
                .FirstOrDefault();

            TimePerDayLookup = context.Entries
                .Where(ent => ent.UserID == userId)
                .Where(ent => startDate <= ent.Date && ent.Date <= endDate)
                .Include(ent => ent.Enrollments)
                .Where(ent => ent.Enrollments.Select(enr => enr.StudentID).Contains(studentID))
                .Where(ent => ent.MinutesSpent >0)
                .GroupBy(ent => ent.Date)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.MinutesSpent));
         }
    }
}
