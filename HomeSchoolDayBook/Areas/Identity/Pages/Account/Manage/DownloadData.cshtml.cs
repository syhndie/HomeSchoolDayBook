using System.Threading.Tasks;
using HomeSchoolDayBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static HomeSchoolDayBook.Helpers.Helpers;

namespace HomeSchoolDayBook.Areas.Identity.Pages.Account.Manage
{
    public class DownloadDataModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;
        private readonly ILogger<DownloadDataModel> _logger;

        public DownloadDataModel(
            ApplicationDbContext context,
            UserManager<HomeSchoolDayBookUser> userManager,
            ILogger<DownloadDataModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                DangerMessage = "Unable to load user.";
                return RedirectToPage();
            }

            _logger.LogInformation("User with ID '{UserId}' asked for data download.", _userManager.GetUserId(User));

            string[] columnHeaders = new string[]
            {
                "Data",
                "Title",
                "Description",
                "Time Spent",
                "Students",
                "Subjects"
            };

            string userId = _userManager.GetUserId(User);
             
            List<Entry> entries = await _context.Entries
               .Where(ent => ent.UserID == userId)
               .Include(ent => ent.Enrollments)
                   .ThenInclude(enr => enr.Student)
               .Include(ent => ent.SubjectAssignments)
                   .ThenInclude(sa => sa.Subject)
               .AsNoTracking()
               .ToListAsync();

            foreach (Entry entry in entries)
            {

            }

            



            return RedirectToPage();
        }
    }
}
