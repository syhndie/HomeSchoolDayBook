using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Models.ViewModels;

namespace HomeSchoolDayBook.Pages.Students
{
    public class GradesModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public Student Student { get; set; }

        public List<SubjectGrade> SubjectGrades { get; set; }

        public GradesModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Student = await _context.Students
                .Where(st => st.UserID == userId)
                .Where(st => st.ID == id)
                .Include(st => st.Grades)
                    .ThenInclude(gr => gr.Subject)
                .Include(st => st.Grades)
                    .ThenInclude(gr => gr.Entry)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (Student == null)
            {
                DangerMessage = "Student not found.";

                return RedirectToPage("./Index");
            }

            SubjectGrades = Student.Grades
                .GroupBy(gr => gr.Subject)
                .Select(x => new SubjectGrade(x.Key, x.Sum(y => y.PointsEarned), x.Sum(y => y.PointsAvailable)))
                .ToList();
                
            return Page();
        }
    }
}