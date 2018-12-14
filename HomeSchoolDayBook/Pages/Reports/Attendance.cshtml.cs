using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models.ViewModels;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class AttendanceModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public List<AttendanceVM> StudentAttendances { get; set; }

        public AttendanceModel(ApplicationDbContext context)
        {
            _context = context;
        }


        public void OnGet(string start, string end, string studentIDs)
        {
            StartDate = Convert.ToDateTime(start);

            EndDate = Convert.ToDateTime(end);

            if (studentIDs == null || studentIDs == "") StudentAttendances = new List<AttendanceVM>();
            else
            {
                StudentAttendances = studentIDs.Split(',')
                    .Select(Int32.Parse)
                    .Select(i => new AttendanceVM(_context, i, StartDate, EndDate))
                    .OrderBy(sa => sa.Student.Name)
                    .ToList();
            }

        }
    }
}