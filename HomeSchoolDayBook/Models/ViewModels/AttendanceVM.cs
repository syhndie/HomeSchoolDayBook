using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeSchoolDayBook.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class AttendanceVM
    {
        public Student Student { get; set; }

        public int DaysAttended { get; set; }

        public int TotalDays { get; set; }

        public AttendanceVM (ApplicationDbContext context, int studentID, DateTime startDate, DateTime endDate, string userId)
        {
            Student = context.Students
                .Where(st => st.UserID == userId)
                .Where(st => st.ID == studentID)
                .FirstOrDefault();

            if (Student == null)
            {
                DaysAttended = 0;
                TotalDays = 0;
            }
            else
            {
                DaysAttended = context.Entries
                    .Include(ent => ent.Enrollments)
                    .Where(ent => ent.UserID == userId)
                    .Where(ent => startDate <= ent.Date && ent.Date <= endDate)
                    .Where(ent => ent.Enrollments.Select(enr => enr.StudentID).Contains(studentID))
                    .Select(ent => ent.Date)
                    .Distinct()
                    .Count();

                TotalDays = ((int)(endDate - startDate).TotalDays) + 1;
            }
        }
    }
}
