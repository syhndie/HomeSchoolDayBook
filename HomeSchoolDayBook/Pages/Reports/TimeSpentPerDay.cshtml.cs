using System;
using System.Collections.Generic;
using System.Linq;
using HomeSchoolDayBook.Data;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Models;
using Microsoft.EntityFrameworkCore;
using static HomeSchoolDayBook.Helpers.Helpers;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class TimeSpentPerDayModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public List<TimeSpentPerDayVM> TimeSpentPerDayVMs { get; set; }

        public TimeSpentPerDayModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public void OnGet(string start, string end, string studentIDs)
        {
            string userId = _userManager.GetUserId(User);

            StartDate = Convert.ToDateTime(start);

            EndDate = Convert.ToDateTime(end);

            if (studentIDs == null || studentIDs == "")  TimeSpentPerDayVMs = new List<TimeSpentPerDayVM>();
            else
            {
                List<int> studentIDList = studentIDs.Split(',')
                    .Select(Int32.Parse)
                    .ToList();

                TimeSpentPerDayVMs = _context.Enrollments
                    .Include(enr => enr.Entry)
                    .Include(enr => enr.Student)
                    .Where(enr => enr.Entry.UserID == userId)
                    .Where(enr => enr.Entry.Date <= EndDate)
                    .Where(enr => StartDate <= enr.Entry.Date)
                    .Where(enr => studentIDList.Contains(enr.StudentID))
                    .ToList()
                    .GroupBy(enr => new { enr.Student, enr.Entry.Date})
                    .ToList()
                    .Select(x => new TimeSpentPerDayVM(x.Key.Student.Name, x.Key.Date, x.Sum(y => (int)(y.Entry.MinutesSpent ?? 0))))
                    .Where(tspd => tspd.MinutesSpent > 0)
                    .OrderBy(tspd => tspd.StudentName)
                    .ThenBy(tspd => tspd.Date)
                    .ToList();
            }
        }
    }
}