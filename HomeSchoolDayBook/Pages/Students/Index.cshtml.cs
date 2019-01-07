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

namespace HomeSchoolDayBook.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public IList<Student> Students { get; set; }

        public IndexModel(HomeSchoolDayBook.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string userId = _userManager.GetUserId(User);

            var adminList = await _userManager.GetUsersInRoleAsync("Admin");

            if (adminList.Select(u => u.Id).Contains(userId))
            {
                Students = await _context.Students
                    .ToListAsync();
            }
            else
            {
                Students = await _context.Students
                    .Where(st => st.UserID == userId)
                    .ToListAsync();
            }       

            return Page();
        }
    }
}
