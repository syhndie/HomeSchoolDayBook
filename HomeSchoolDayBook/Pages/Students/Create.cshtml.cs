using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Data;

namespace HomeSchoolDayBook.Pages.Students
{
    public class CreateModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public StudentOrSubjectEditOrCreate StudentVM { get; set; }
    
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

            StudentVM = new StudentOrSubjectEditOrCreate();

            bool modelDidUpdate = await TryUpdateModelAsync<StudentOrSubjectEditOrCreate>(StudentVM);

            if (ModelState.IsValid && modelDidUpdate) 
            {
                List<string> usedNames = _context.Students
                    .Where(st => st.UserID == userId)
                    .Select(st => st.Name)
                    .ToList();

                if (usedNames.Contains(StudentVM.Name))
                {
                    DangerMessage = "This Student name is already used.";

                    return RedirectToPage();
                }

                Student newStudent = new Student
                {
                    UserID = userId,
                    Name = StudentVM.Name,
                    IsActive = StudentVM.IsActive
                };

                _context.Students.Add(newStudent);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DangerMessage = "New Student did not save correctly. Please try again.";

            return RedirectToPage();
        }
    }
}