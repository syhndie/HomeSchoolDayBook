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
    public class GradeSummariesModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        [BindProperty (SupportsGet = true)]
        [DataType(DataType.Date)]
        [Display(Name = "From")]
        public DateTime FromDate { get; set; }

        [BindProperty (SupportsGet = true)]
        [DataType(DataType.Date)]
        [Display(Name ="To")]
        public DateTime ToDate { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name ="Student")]
        public int StudentID { get; set; }

        public List<SelectListItem> StudentOptions { get; set; }

        public List<SubjectGradeVM> SubjectGrades { get; set; }

        public GradeSummariesModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(DateTime? fromDate, DateTime? toDate, int studentID)
        {
            string userId = _userManager.GetUserId(User);

            if (fromDate == null) FromDate = DateTime.Today.AddMonths(-1);

            if (toDate == null)  ToDate = DateTime.Today;

            StudentOptions = await _context
                .Students
                .Where(st => st.UserID == userId)
                .Where(st => st.IsActive)
                .OrderBy(st => st.Name)
                .Select(st => new SelectListItem { Value = st.ID.ToString(), Text = st.Name })
                .ToListAsync();

            DateTime startDate = FromDate <= ToDate ? (DateTime)FromDate : (DateTime)ToDate;
            DateTime endDate = startDate == (DateTime)FromDate ? (DateTime)ToDate : (DateTime)FromDate;

            SubjectGrades = await _context
                .Grades
                .Where(gr => gr.StudentID == studentID)
                .Where(gr => gr.Entry.Date >= startDate)
                .Where(gr => gr.Entry.Date <= endDate)
                .GroupBy(gr=> gr.Subject)
                .Select(x => new SubjectGradeVM(x.Key, x.Sum(y => y.PointsEarned), x.Sum(y => y.PointsAvailable)))
                .ToListAsync();

            return Page();
        }
    }
}