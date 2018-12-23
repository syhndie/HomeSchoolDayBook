using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeSchoolDayBook.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Models.ViewModels;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class TimeSpentPerDayModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public List<TimeSpentPerDayVM> StudentTimeSpents { get; set; }

        public TimeSpentPerDayModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(string start, string end, string studentIDs)
        {
            StartDate = Convert.ToDateTime(start);

            EndDate = Convert.ToDateTime(end);

            if (studentIDs == null || studentIDs == "")  StudentTimeSpents = new List<TimeSpentPerDayVM>();
            else
            {
                StudentTimeSpents = studentIDs.Split(',')
                    .Select(Int32.Parse)
                    .Select(i => new TimeSpentPerDayVM(_context, i, StartDate, EndDate))
                    .OrderBy(sa => sa.Student.Name)
                    .ToList();
            }

         

        }
    }
}