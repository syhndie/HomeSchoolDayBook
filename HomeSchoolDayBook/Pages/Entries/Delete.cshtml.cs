using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeSchoolDayBook.Pages.Entries
{
    public class DeleteModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public Entry Entry { get; set; }

        [TempData]
        public string NotFoundMessage { get; set; }

        public DeleteModel(HomeSchoolDayBook.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Entry = await _context.Entries
                .Where(ent => ent.UserID == userId)
                .Where(ent => ent.ID == id)
                .Include(ent => ent.Enrollments)
                    .ThenInclude(enr => enr.Student)
                .Include(ent => ent.SubjectAssignments)
                    .ThenInclude(sa => sa.Subject)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (Entry == null)
            {
                NotFoundMessage = "Entry not found.";

                return RedirectToPage("./Index");
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Entry = await _context.Entries
                .Where(ent => ent.UserID == userId)
                .Where(ent => ent.ID == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (Entry == null)
            {
                return RedirectToPage("./Index");
            }

            _context.Remove(Entry);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
