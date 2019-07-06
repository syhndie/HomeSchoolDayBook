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
                DangerMessage = "Subject not found.";

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
                DangerMessage = "Subject not found.";

                return RedirectToPage("./Index");
            }

            bool modelDidUpdate = await TryUpdateModelAsync<Subject>(Subject);

            if (ModelState.IsValid && modelDidUpdate)
            {
                List<string> otherUsedNames = await _context.Subjects
                    .Where(su => su.ID != id)
                    .Where(su => su.UserID == userId)
                    .Select(su => su.Name)
                    .ToListAsync();

                if (otherUsedNames.Contains(Subject.Name))
                {
                    DangerMessage = "This Subject name is already used.";

                    return RedirectToPage();
                }

                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            DangerMessage = "Changes did not save correctly. Please try again.";

            return RedirectToPage();
        }

    }
}
