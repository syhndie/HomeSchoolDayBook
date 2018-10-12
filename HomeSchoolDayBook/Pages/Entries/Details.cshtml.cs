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
    public class DetailsModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public DetailsModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Entry Entry { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Entry = await _context.Entries
                .Include(ent => ent.Enrollments)
                .ThenInclude(enr => enr.Student)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Entry == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
