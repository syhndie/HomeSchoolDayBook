using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Pages.Students
{
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        public bool IsActive { get; set; }

        public EditModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Student student = await _context.Students
                .Where(st => st.UserID == userId)
                .Where(st => st.ID == id)
                .SingleOrDefaultAsync();

            if (student == null)
            {
                DangerMessage = "Student not found.";

                return RedirectToPage("./Index");
            }

            Name = student.Name;
            IsActive = student.IsActive;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Student student = await _context.Students
                .Where(st => st.UserID == userId)
                .Where(st => st.ID == id)
                .SingleOrDefaultAsync();

            if (student == null)
            {
                DangerMessage = "Student not found.";

                return RedirectToPage("./Index");
            }

            student.Name = Name;
            student.IsActive = IsActive;

            if (ModelState.IsValid)
            {
                List<string> otherUsedNames = _context.Students
                    .Where(st => st.ID != id)
                    .Where(st => st.UserID == userId)
                    .Select(st=> st.Name)
                    .ToList();

                if (otherUsedNames.Contains(Name))
                {
                    DangerMessage = "This Student name is already used.";

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