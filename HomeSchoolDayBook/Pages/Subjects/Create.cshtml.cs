using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Pages.Subjects
{
    public class CreateModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public Subject Subject { get; set; }

        public string ErrorMessage { get; set; }

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
            Subject newSubject = new Subject();

            bool modelDidUpdate = await TryUpdateModelAsync<Subject>(newSubject, "subject");

            if (ModelState.IsValid && modelDidUpdate)
            {
                _context.Subjects.Add(newSubject);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            ErrorMessage = "New Subject did not save correctly. Please try again";

            return Page();
        }
    }
}