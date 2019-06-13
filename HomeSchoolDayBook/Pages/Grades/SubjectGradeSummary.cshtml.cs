using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace HomeSchoolDayBook.Pages.Grades
{
    public class SubjectGradeSummaryModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        [BindProperty (SupportsGet = true)]
        [DataType(DataType.Date)]
        [Display(Name = "From")]
        public DateTime FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        [Display(Name = "To")]
        public DateTime ToDate { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Subject")]
        public int SubjectID { get; set; }

        public List<SelectListItem> SubjectOptions { get; set; }

        public List<StudentGradeVM> StudentGrades { get; set; }

        public SubjectGradeSummaryModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(DateTime? fromDate, DateTime? toDate, int subjectID)
        {
            string userId = _userManager.GetUserId(User);

            if (fromDate == null) FromDate = DateTime.Today.AddMonths(-1);

            if (toDate == null) ToDate = DateTime.Today;

            SubjectOptions = await _context
                .Subjects
                .Where(su => su.UserID == userId)
                .Where(su => su.IsActive)
                .OrderBy(su => su.Name)
                .Select(su => new SelectListItem { Value = su.ID.ToString(), Text = su.Name })
                .ToListAsync();

            DateTime startDate = FromDate <= ToDate ? (DateTime)FromDate : (DateTime)ToDate;
            DateTime endDate = startDate == (DateTime)FromDate ? (DateTime)ToDate : (DateTime)FromDate;

            StudentGrades = await _context
                .Grades
                .Where(gr => gr.UserID == userId)
                .Where(gr => gr.SubjectID == subjectID)
                .Where(gr => gr.Entry.Date >= startDate)
                .Where(gr => gr.Entry.Date <= endDate)
                .GroupBy(gr => gr.Student)
                .Select(x => new StudentGradeVM(x.Key, x.Sum(y => y.PointsEarned), x.Sum(y => y.PointsAvailable)))
                .ToListAsync();

            return Page();
        }
    }
}