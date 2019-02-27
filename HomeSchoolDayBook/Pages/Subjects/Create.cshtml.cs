using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Data;

namespace HomeSchoolDayBook.Pages.Subjects
{
    public class CreateModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public Subject Subject { get; set; }


        public CreateModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            string userId = _userManager.GetUserId(User);

            Subject = new Subject
            {
                UserID = userId
            };

            bool modelDidUpdate = await TryUpdateModelAsync<Subject>(Subject);

            if (ModelState.IsValid && modelDidUpdate)
            {
                List<string> usedNames = _context.Subjects
                    .Where(su => su.UserID == userId)
                    .Select(s => s.Name)
                    .ToList();

                if (usedNames.Contains(Subject.Name))
                {
                    DangerMessage = "This Subject name is already used.";

                    return RedirectToPage();
                }

                _context.Subjects.Add(Subject);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DangerMessage = "New Subject did not save correctly. Please try again";

            return RedirectToPage();
        }
    }
}