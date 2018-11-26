using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Pages.Subjects
{
    public class DetailsModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public Subject Subject { get; set; }

        [TempData]
        public string NotFoundMessage { get; set; }

        public DetailsModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Subject = await _context.Subjects.FirstOrDefaultAsync(m => m.ID == id);

            if (Subject == null)
            {
                NotFoundMessage = "Subject not found. The Subject you selected is no longer in the database.";
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
