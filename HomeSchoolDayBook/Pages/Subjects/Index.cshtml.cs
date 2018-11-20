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
    public class IndexModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public IList<Subject> Subjects { get; set; }

        public IndexModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }              

        public async Task<IActionResult> OnGetAsync()
        {
            Subjects = await _context.Subjects.ToListAsync();

            return Page();
        }
    }
}
