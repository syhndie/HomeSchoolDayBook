using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeSchoolDayBook.Pages.Subjects
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public Subject Subject { get; set; }

        public string DidNotSaveMessage { get; set; }

        public CreateModel(HomeSchoolDayBook.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Subject = new Subject
            {
                UserID = _userManager.GetUserId(User)
            };

            bool modelDidUpdate = await TryUpdateModelAsync<Subject>(Subject);

            if (ModelState.IsValid && modelDidUpdate)
            {
                List<string> usedNames = _context.Subjects.Select(s => s.Name).ToList();

                if (usedNames.Contains(Subject.Name))
                {
                    DidNotSaveMessage = "This Subject is already in the database.";

                    return Page();
                }

                _context.Subjects.Add(Subject);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DidNotSaveMessage = "New Subject did not save correctly. Please try again";

            return Page();
        }
    }
}