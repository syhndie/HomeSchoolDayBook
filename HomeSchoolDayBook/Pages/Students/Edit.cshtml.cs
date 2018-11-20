using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public Student Student { get; set; }

        public string ErrorMessage {get; set;}

        public EditModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Student = await _context.Students.FirstOrDefaultAsync(m => m.ID == id);

            if (Student == null)
            {
                ErrorMessage = "Subject not found. Please try again.";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            Student editedStudent = await _context.Students.FirstOrDefaultAsync(m => m.ID == id);

            if (editedStudent == null)
            {
                ErrorMessage = "Student not found. Please try again.";

                return Page();
            }

            bool modelDidUpdate = await TryUpdateModelAsync<Student>(editedStudent, "student");

            if (ModelState.IsValid && modelDidUpdate)
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            ErrorMessage = "Changes did not save correctly. Please try again.";
            return Page();
        }
    }
}
