using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Data;

namespace HomeSchoolDayBook.Pages.Subjects
{
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        public Subject Subject { get; set; }

        [TempData]
        public string NotFoundMessage { get; set; }

        public string DidNotSaveMessage { get; set; }

        public EditModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }   

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Subject = await _context.Subjects
                .Where(su => su.UserID == userId)
                .Where(su => su.ID == id)
                .FirstOrDefaultAsync();

            if (Subject == null)
            {
                NotFoundMessage = "Subject not found.";

                return RedirectToPage("./Index");
            }

            return Page();
            
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Subject = await _context.Subjects
                .Where(su => su.UserID == userId)
                .Where(su => su.ID == id)
                .FirstOrDefaultAsync();

            if (Subject == null)
            {
                NotFoundMessage = "Subject not found.";

                return RedirectToPage("./Index");
            }

            bool modelDidUpdate = await TryUpdateModelAsync<Subject>(Subject);

            if (ModelState.IsValid && modelDidUpdate)
            {
                List<string> otherUsedNames = _context.Subjects
                    .Where(su => su.ID != id)
                    .Where(su => su.UserID == userId)
                    .Select(su => su.Name)
                    .ToList();

                if (otherUsedNames.Contains(Subject.Name))
                {
                    DidNotSaveMessage = "This Subject name is already used.";

                    return Page();
                }

                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            DidNotSaveMessage = "Changes did not save correctly. Please try again.";

            return Page();
        }

    }
}
