using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace HomeSchoolDayBook.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public Student Student { get; set; }
    
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
                    DidNotSaveMessage = "This Student name is already used.";

                    return Page();
                }
                _context.Students.Add(Student);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DidNotSaveMessage = "New Student did not save correctly. Please try again.";

            return Page();
        }
    }
}