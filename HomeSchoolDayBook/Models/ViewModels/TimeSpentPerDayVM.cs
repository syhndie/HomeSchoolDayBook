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

        public TimeSpentPerDayVM(ApplicationDbContext context, int studentID, DateTime startDate, DateTime endDate)
        {
            Student = context.Students.FirstOrDefault(m => m.ID == studentID);

            TimePerDayLookup = context.Entries
                .Include(ent => ent.Enrollments)
                .Where(ent => startDate <= ent.Date && ent.Date <= endDate)
                .Where(ent => ent.Enrollments.Select(enr => enr.StudentID).Contains(studentID))
                .Where(ent => ent.MinutesSpent >0)
                .GroupBy(ent => ent.Date)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.MinutesSpent));
         }
    }
}
