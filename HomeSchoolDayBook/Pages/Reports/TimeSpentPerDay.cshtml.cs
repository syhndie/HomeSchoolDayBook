using System;
using System.Collections.Generic;
using System.Linq;
using HomeSchoolDayBook.Data;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Models;

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

        public List<TimeSpentPerDayVM> StudentTimeSpents { get; set; }

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

            if (studentIDs == null || studentIDs == "")  StudentTimeSpents = new List<TimeSpentPerDayVM>();
            else
            {
                StudentTimeSpents = studentIDs.Split(',')
                    .Select(Int32.Parse)
                    .Select(id => new TimeSpentPerDayVM(_context, id, StartDate, EndDate, userId))
                    .Where(stp => stp.Student != null)
                    .OrderBy(vm => vm.Student.Name)
                    .ToList();
            }

         

        }
    }
}