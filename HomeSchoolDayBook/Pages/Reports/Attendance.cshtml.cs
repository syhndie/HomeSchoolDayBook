using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class AttendanceModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public int TotalDays { get; set; }

        public List<AttendanceVM> StudentAttendances { get; set; }

        public AttendanceModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public void OnGet(string start, string end, string studentIDs)
        {
            string userId = _userManager.GetUserId(User);

            StartDate = Convert.ToDateTime(start);

            EndDate = Convert.ToDateTime(end);

            TotalDays = (int)(EndDate - StartDate).TotalDays + 1;

            if (studentIDs == null || studentIDs == "") StudentAttendances = new List<AttendanceVM>();
            else
            {
                List<int> studentIDList = studentIDs.Split(',')
                    .Select(Int32.Parse)
                    .ToList();

                List<Student> studentList = _context.Students.
                    Where(st => st.UserID == userId)
                    .Where(st => studentIDList.Contains(st.ID))
                    .ToList();

                List<Enrollment> enrollments = _context.Enrollments
                    .Include(enr => enr.Entry)
                    .Include(enr => enr.Student)
                    .Where(enr => enr.Entry.UserID == userId)
                    .Where(enr => enr.Entry.Date <= EndDate)
                    .Where(enr => StartDate <= enr.Entry.Date)
                    .Where(enr => studentIDList.Contains(enr.StudentID))
                    .ToList();

                StudentAttendances = enrollments
                    .Select(enr => new { enr.Student.Name, enr.Entry.Date })
                    .Distinct()
                    .ToList()
                    .GroupBy(a => a.Name)
                    .Select(x => new AttendanceVM(x.Key, x.Count()))
                    .ToList();

                foreach (Student student in studentList)
                {
                    if (!enrollments.Select(enr => enr.Student).Contains(student))
                    {
                        StudentAttendances.Add(new AttendanceVM(student.Name, 0));
                    }
                }
            }
        }
    }
}