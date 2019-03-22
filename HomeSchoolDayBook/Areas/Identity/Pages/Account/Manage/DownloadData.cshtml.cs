using System.Threading.Tasks;
using HomeSchoolDayBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Models.ViewModels;
using HomeSchoolDayBook.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static HomeSchoolDayBook.Helpers.Helpers;
using System.IO;
using CsvHelper;

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

            //use csvhelper
            //this may allow me to just have a list of downloadEntries and it will write based on the properties.
            //try that first.
            //if not, I might need to make a list of string arrays

            //make each entry into an array of strings, one string for each column
            string[] columnHeaders = new string[]
            {
                "Title",
                "Date",
                "Description",
                "Minutes Spent",
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

            //write to the csv file as you loop through the entries
            using (var writer = new StreamWriter(@"C:\Users\Cindy\Documents\Visual Studio 2017\Projects\HomeSchoolDayBook\HomeSchoolDayBook\wwwroot\HSDBData.csv"))
            using (var csv = new CsvWriter(writer))
            {
                foreach (Entry entry in entries)
                {
                    var download = new
                    {
                        Date = entry.Date.ToShortDateString(),
                        Title = entry.Title,
                        Description = entry.Description,
                        Minutes = entry.MinutesSpent,
                        Students = GetStudentNames(entry),
                        Subjects = GetSubjectNames(entry)
                    };

                    csv.WriteRecord(download);
                    csv.NextRecord();
                }

            }


            //        void Main()
            //        {
            //            var records = new List<object>
            //{
            //    new { Id = 1, Name = "one" },
            //};

            //            using (var writer = new StreamWriter("path\\to\\file.csv"))
            //            using (var csv = new CsvWriter(writer))
            //            {
            //                csv.WriteRecords(records);
            //            }
            //        }





            return RedirectToPage();
        }
    }
}
