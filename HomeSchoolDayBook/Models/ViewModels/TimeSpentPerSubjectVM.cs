using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeSchoolDayBook.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class TimeSpentPerSubjectVM
    {
        public Student Student { get; set; }

        public Dictionary<Subject, int?> TimePerSubjectLookup { get; set; }

        public TimeSpentPerSubjectVM(ApplicationDbContext context, int studentID, DateTime startDate, DateTime endDate)
        {
            Student = context.Students.FirstOrDefault(m => m.ID == studentID);

            TimePerSubjectLookup = context.Entries
                .Include(ent => ent.SubjectAssignments)
                .ThenInclude(sa => sa.Subject)
                .Include(ent => ent.Enrollments)
                .Where(ent => startDate <= ent.Date && ent.Date <= endDate)
                .Where(ent => ent.Enrollments.Select(enr => enr.StudentID).Contains(studentID))
                .Where(ent => ent.MinutesSpent > 0)
                .Where(ent => ent.SubjectAssignments != null)
                .SelectMany(ent => ent.SubjectAssignments.Select(sa => sa.Subject), (ent, sub) => new { sub, ent.MinutesSpent })
                .GroupBy(a => a.sub)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.MinutesSpent));
        }
    }
}
