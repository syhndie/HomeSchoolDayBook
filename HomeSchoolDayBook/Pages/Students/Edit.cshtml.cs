using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;

namespace HomeSchoolDayBook.Pages.Students
{
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        public Student Student { get; set; }

        public EditModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Student = await _context.Students
                .Where(st => st.UserID == userId)
                .Where(st => st.ID == id)
                .FirstOrDefaultAsync();

            if (Student == null)
            {
                DangerMessage = "Student not found.";

                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Student = await _context.Students
                .Where(st => st.UserID == userId)
                .Where(st => st.ID == id)
                .FirstOrDefaultAsync();

            if (Student == null)
            {
                DangerMessage = "Student not found.";

                return RedirectToPage("./Index");
            }

            bool modelDidUpdate = await TryUpdateModelAsync<Student>(Student);

            if (ModelState.IsValid && modelDidUpdate)
            {
                List<string> otherUsedNames = _context.Students
                    .Where(st => st.ID != id)
                    .Where(st => st.UserID == userId)
                    .Select(st=> st.Name)
                    .ToList();

                if (otherUsedNames.Contains(Student.Name))
                {
                    DangerMessage = "This Student name is already used.";

                    return RedirectToPage();
                }

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DangerMessage = "Changes did not save correctly. Please try again.";

            return Page();
        }
    }
}