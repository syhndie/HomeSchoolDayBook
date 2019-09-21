using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Data;
using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Pages.Entries
{
    public class IndexModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        [Display(Name = "From")]
        public DateTime FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        [Display(Name = "To")]
        public DateTime ToDate { get; set; }

        public List<Entry> Entries { get;set; }

        public async Task<IActionResult> OnGetAsync(DateTime? fromDate, DateTime? toDate)
        {
            string userId = _userManager.GetUserId(User);

            if (fromDate == null) FromDate = DateTime.Today.AddMonths(-1).AddDays(1);

            if (toDate == null) ToDate = DateTime.Today;

            DateTime startDate = FromDate <= ToDate ? (DateTime)FromDate : (DateTime)ToDate;
            DateTime endDate = startDate == (DateTime)FromDate ? (DateTime)ToDate : (DateTime)FromDate;

            Entries = await _context.Entries
                .Where(ent => ent.UserID == userId)
                .Where(ent => ent.Date >= startDate)
                .Where(ent => ent.Date <= endDate)
                .Include(ent => ent.Enrollments)
                    .ThenInclude(enr => enr.Student)
                .Include(ent => ent.SubjectAssignments)
                    .ThenInclude(sa => sa.Subject)
                .OrderByDescending(ent => ent.Date)
                    .ThenBy(ent => ent.Title)
                .AsNoTracking()
                .ToListAsync();

            return Page();
        }
    }
}
