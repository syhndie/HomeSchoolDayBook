using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;

namespace HomeSchoolDayBook.Pages.Students
{
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        public StudentOrSubjectEditOrCreate StudentVM { get; set; }

        public EditModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Student studentToEdit = await _context.Students
                .Where(st => st.UserID == userId)
                .Where(st => st.ID == id)
                .SingleOrDefaultAsync();

            if (studentToEdit == null)
            {
                DangerMessage = "Student not found.";

                return RedirectToPage("./Index");
            }

            StudentVM = new StudentOrSubjectEditOrCreate
            {
                Name = studentToEdit.Name,
                IsActive = studentToEdit.IsActive
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Student studentToEdit = await _context.Students
                .Where(st => st.UserID == userId)
                .Where(st => st.ID == id)
                .SingleOrDefaultAsync();

            if (studentToEdit == null)
            {
                DangerMessage = "Student not found.";

                return RedirectToPage("./Index");
            }

            StudentVM = new StudentOrSubjectEditOrCreate();

            bool modelDidUpdate = await TryUpdateModelAsync<StudentOrSubjectEditOrCreate>(StudentVM);

            if (ModelState.IsValid && modelDidUpdate)
            {
                List<string> otherUsedNames = _context.Students
                    .Where(st => st.ID != id)
                    .Where(st => st.UserID == userId)
                    .Select(st=> st.Name)
                    .ToList();

                if (otherUsedNames.Contains(StudentVM.Name))
                {
                    DangerMessage = "This Student name is already used.";

                    return RedirectToPage();
                }

                studentToEdit.Name = StudentVM.Name;
                studentToEdit.IsActive = StudentVM.IsActive;

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DangerMessage = "Changes did not save correctly. Please try again.";

            return Page();
        }
    }
}