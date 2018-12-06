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

        public AttendanceVM (ApplicationDbContext context, int studentID, DateTime startDate, DateTime endDate)
        {
            Student = context.Students.FirstOrDefault(m => m.ID == studentID);

            DaysAttended = context.Entries
                .Include(ent => ent.Enrollments)
                .Where(ent => startDate <= ent.Date && ent.Date <= endDate)
                .Where(ent => ent.Enrollments.Select(enr => enr.StudentID).Contains(studentID))
                .Select(ent => ent.Date)
                .Distinct()
                .Count();

            TotalDays = (int)(endDate - startDate).TotalDays;
        }
    }
}
