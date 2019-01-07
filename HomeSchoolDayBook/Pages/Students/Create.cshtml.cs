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
            Student = new Student
            {
                UserID = _userManager.GetUserId(User)
            };

            bool modelDidUpdate = await TryUpdateModelAsync<Student>(Student);

            if (ModelState.IsValid && modelDidUpdate) 
            {
                List<string> usedNames = _context.Students.Select(s => s.Name).ToList();

                if (usedNames.Contains(Student.Name))
                {
                    DidNotSaveMessage = "This Student is already in the database.";

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