using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HomeSchoolDayBook.Pages.Students
{
    public class CreateModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        [Display(Name = "Active")]
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    
        public CreateModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public void OnGet()
        {
            IsActive = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string userId = _userManager.GetUserId(User);

            Student student = new Student
            {
                UserID = userId, Name = Name, IsActive = IsActive
            };

            if (ModelState.IsValid) 
            {
                List<string> usedNames = _context.Students
                    .Where(st => st.UserID == userId)
                    .Select(st => st.Name)
                    .ToList();

                if (usedNames.Contains(Name))
                {
                    DangerMessage = "This Student name is already used.";

                    return RedirectToPage();
                }
                _context.Students.Add(student);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DangerMessage = "New Student did not save correctly. Please try again.";

            return RedirectToPage();
        }
    }
}