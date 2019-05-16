using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Data;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class IndexModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public readonly List<string> ReportViews = new List<string>
        { "./Attendance", "./TimeSpentPerDay", "./EntriesInBrief",  "./TimeSpentPerSubject", "./EntriesInFull", "./SubjectGrades"};

        [DataType(DataType.Date)]
        [Display(Name ="From")]
        public DateTime FromDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name ="To")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Students")]
        public List<CheckBoxVM> StudentCheckBoxes { get; set; }

        public IndexModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            string userId = _userManager.GetUserId(User);

            FromDate = DateTime.Today.AddMonths(-1);

            ToDate = DateTime.Today;

            StudentCheckBoxes = await _context
                .Students
                .Where(st => st.UserID == userId)
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

            if (!ReportViews.Contains(selectedReport))
            {
                DangerMessage = "An error occurred when selecting a report to view.";
                return RedirectToPage();
            }

            return RedirectToPage(selectedReport, new { start = startDate, end = endDate, studentIDs = selectedStudentsAsString });
        }
    }
}