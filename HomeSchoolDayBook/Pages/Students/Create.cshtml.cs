using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public Student Student { get; set; }

        public string DidNotUpdateMessage { get; set; }

        public CreateModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Student newStudent = new Student();

            bool modelDidUpdate = await TryUpdateModelAsync<Student>(newStudent, "student");

            if (ModelState.IsValid && modelDidUpdate) 
            {
                _context.Students.Add(newStudent);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DidNotUpdateMessage = "New Student did not save correctly. Please try again.";
            return Page();
        }
    }
}