using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models.ViewModels;
using System.Text.RegularExpressions;


using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public readonly List<string> ReportViews = new List<string>
        { "./Attendance", "./TimeSpentPerDay", "./EntriesInBrief",  "./TimeSpentPerSubject", "./EntriesInFull"};

        [DataType(DataType.Date)]
        [Display(Name ="From")]
        public DateTime FromDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name ="To")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Students")]
        public List<CheckBoxVM> StudentCheckBoxes { get; set; }

        public IndexModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            FromDate = DateTime.Today.AddMonths(-1);

            ToDate = DateTime.Today;

            StudentCheckBoxes = await _context
                .Students
                .Where(st => st.IsActive)
                .OrderBy(st => st.Name)
                .Select(st => new CheckBoxVM(st.ID, st.Name, false))
                .ToListAsync();

            return Page();            
        }

        public IActionResult OnPost(string fromDate, string toDate, string[] selectedStudents, string selectedReport)
        { 
            string startDate = Convert.ToDateTime(fromDate) <= Convert.ToDateTime(toDate) ? fromDate : toDate;

            string endDate = startDate == fromDate ? toDate : fromDate;

            string selectedStudentsAsString = String.Join(',', selectedStudents);

            if (ReportViews.Contains(selectedReport))
            {
                return RedirectToPage(selectedReport, new { start = startDate, end = endDate, studentIDs = selectedStudentsAsString });
            }
            else return RedirectToPage("./NoReport");
        }
    }
}