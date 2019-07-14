using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Data;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Pages.Subjects
{
    public class CreateModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        public bool IsActive { get; set; }

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

            Subject subject = new Subject
            {
                UserID = userId, Name = Name, IsActive = IsActive
            };
            

            if (ModelState.IsValid)
            {
                List<string> usedNames = _context.Subjects
                    .Where(su => su.UserID == userId)
                    .Select(s => s.Name)
                    .ToList();

                if (usedNames.Contains(Name))
                {
                    DangerMessage = "This Subject name is already used.";

                    return RedirectToPage();
                }

                _context.Subjects.Add(subject);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DangerMessage = "New Subject did not save correctly. Please try again";

            return RedirectToPage();
        }
    }
}