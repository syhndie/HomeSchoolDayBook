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

namespace HomeSchoolDayBook.Pages.Subjects
{
    public class EditModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public Subject Subject { get; set; }

        [TempData]
        public string NotFoundMessage { get; set; }

        public string DidNotSaveMessage { get; set; }

        public EditModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            Subject editedSubject = await _context.Subjects.FirstOrDefaultAsync(m => m.ID == id);

            if (editedSubject == null)
            {
                NotFoundMessage = "Subject not found. The Subject you selected is no longer in the database.";

                return RedirectToPage("./Index");
            }

            bool modelDidUpdate = await TryUpdateModelAsync<Subject>(editedSubject, "subject");

            //if (ModelState.IsValid && modelDidUpdate)
            //{
            //    await _context.SaveChangesAsync();
            //    return RedirectToPage("./Index");
            //}

            DidNotSaveMessage = "Changes did not save correctly. Please try again.";
            return Page();
        }

    }
}
