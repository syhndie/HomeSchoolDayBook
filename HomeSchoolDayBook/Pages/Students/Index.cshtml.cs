using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;

namespace HomeSchoolDayBook.Pages.Students
{
    public class IndexModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public IList<Student> Students { get; set; }

        public IndexModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async void OnGetAsync()
        {
            string userId = _userManager.GetUserId(User);

            Students = await _context.Students
                .Where(st => st.UserID == userId)
                .ToListAsync();
        }
    }
}
