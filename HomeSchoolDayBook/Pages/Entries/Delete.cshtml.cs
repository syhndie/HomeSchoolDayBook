using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Pages.Entries
{
    public class DeleteModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public Entry Entry { get; set; }

        [TempData]
        public string NotFoundMessage { get; set; }

        public DeleteModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Entry = await _context.Entries
                .Include(ent => ent.Enrollments)
                    .ThenInclude(enr => enr.Student)
                .Include(ent => ent.SubjectAssignments)
                    .ThenInclude(sa => sa.Subject)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Entry == null)
            {
                NotFoundMessage = "Entry not found. The Entry you selected is no longer in the database.";

                return RedirectToPage("./Index");
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            Entry = await _context.Entries
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ID == id);

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
