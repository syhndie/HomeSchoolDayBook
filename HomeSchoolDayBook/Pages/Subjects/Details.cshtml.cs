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

        public string ErrorMessage { get; set; }

        public DetailsModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Subject = await _context.Subjects.FirstOrDefaultAsync(m => m.ID == id);

            if (Subject == null)
            {
                ErrorMessage = "Subject not found. Please try again.";
            }
            return Page();
        }
    }
}
