using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;

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

            if (studentIDs == null || studentIDs == "") StudentAttendances = new List<AttendanceVM>();
            else
            {
                StudentAttendances = studentIDs.Split(',')
                    .Select(Int32.Parse)
                    .Select(id => new AttendanceVM(_context, id, StartDate, EndDate, userId))
                    .Where(sa => sa.Student != null)
                    .OrderBy(sa => sa.Student.Name)
                    .ToList();                
            }
        }
    }
}