using HomeSchoolDayBook.Models;
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

        public Student Student { get; set; }
    
        public CreateModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
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
            string userId = _userManager.GetUserId(User);

            Student = new Student
            {
                UserID = userId
            };

            bool modelDidUpdate = await TryUpdateModelAsync<Student>(Student);

            if (ModelState.IsValid && modelDidUpdate) 
            {
                List<string> usedNames = _context.Students
                    .Where(st => st.UserID == userId)
                    .Select(st => st.Name)
                    .ToList();

                if (usedNames.Contains(Student.Name))
                {
                    DangerMessage = "This Student name is already used.";

                    return RedirectToPage();
                }
                _context.Students.Add(Student);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DangerMessage = "New Student did not save correctly. Please try again.";

            return RedirectToPage();
        }
    }
}