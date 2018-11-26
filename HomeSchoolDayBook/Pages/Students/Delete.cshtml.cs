using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Pages.Students
{
    public class DeleteModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public Student Student { get; set; }

        [TempData]
        public string NotFoundMessage { get; set; }

        public DeleteModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Student = await _context.Students.FirstOrDefaultAsync(m => m.ID == id);

            if (Student == null)
            {
                NotFoundMessage = "Student not found. The Student you selected is no longer in the database.";

                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
             Student = await _context.Students.FirstOrDefaultAsync(m => m.ID == id);

            if (Student == null)
            {
                return RedirectToPage("./Index");
            }

            _context.Students.Remove(Student);

            await _context.SaveChangesAsync();
                       
            return RedirectToPage("./Index");
        }
    }
}
