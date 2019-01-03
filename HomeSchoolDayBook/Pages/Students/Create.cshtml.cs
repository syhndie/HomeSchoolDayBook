using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace HomeSchoolDayBook.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public Student Student { get; set; }
    
        public string DidNotSaveMessage { get; set; }

        public CreateModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Student = new Student();

            bool modelDidUpdate = await TryUpdateModelAsync<Student>(Student);
//            bool modelDidUpdate = await TryUpdateModelAsync<Student>(Student, "student");

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