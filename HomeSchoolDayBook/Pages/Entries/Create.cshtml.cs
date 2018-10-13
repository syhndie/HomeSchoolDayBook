using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Pages.Entries
{
    public class CreateModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public CreateModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.Entry Entry { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyEntry = new Entry();

            if (await TryUpdateModelAsync<Entry>(
                emptyEntry,
                "entry",
                e => e.Date,
                e => e.Title,
                e => e.MinutesSpent,
                e => e.Description))
            {
                _context.Entries.Add(emptyEntry);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return null;
        }       
    }
}