using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Data;

namespace HomeSchoolDayBook.Pages.Subjects
{
    public class CreateModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public StudentOrSubjectEditOrCreate  SubjectVM {get; set;}

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

            SubjectVM = new StudentOrSubjectEditOrCreate();

            bool modelDidUpdate = await TryUpdateModelAsync<StudentOrSubjectEditOrCreate>(SubjectVM);

            if (modelDidUpdate && ModelState.IsValid)
            {
                List<string> usedNames = _context.Subjects
                    .Where(su => su.UserID == userId)
                    .Select(s => s.Name)
                    .ToList();

                if (usedNames.Contains(SubjectVM.Name))
                {
                    DangerMessage = "This Subject name is already used.";

                    return RedirectToPage();
                }

                Subject newSubject = new Subject
                {
                    UserID = userId,
                    Name = SubjectVM.Name,
                    IsActive = SubjectVM.IsActive
                };

                _context.Subjects.Add(newSubject);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DangerMessage = "New Subject did not save correctly. Please try again";

            return RedirectToPage();
        }
    }
}