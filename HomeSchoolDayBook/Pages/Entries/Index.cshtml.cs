using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;

namespace HomeSchoolDayBook.Pages.Entries
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public IndexModel(HomeSchoolDayBook.Data.ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IList<Entry> Entries { get;set; }

        public async Task OnGetAsync()
        {
            string userId = _userManager.GetUserId(User);

            Entries = await _context.Entries
                .Where(ent => ent.UserID == userId)
                .Include(ent => ent.Enrollments)
                    .ThenInclude(enr => enr.Student)
                .Include(ent => ent.SubjectAssignments)
                    .ThenInclude(sa => sa.Subject)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
