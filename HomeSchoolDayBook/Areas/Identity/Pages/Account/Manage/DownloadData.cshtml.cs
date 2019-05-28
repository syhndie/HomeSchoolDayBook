using System.Threading.Tasks;
using System;
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
using System.IO;
using CsvHelper;
using System.Text;
using System.Linq;

namespace HomeSchoolDayBook.Areas.Identity.Pages.Account.Manage
{
    public class DownloadDataModel : BasePageModel
    {
        private readonly static string DateStamp = DateTime.Now.ToString("yyyyMMdd");
        private readonly string FileName = $"HSDBData{DateStamp}.csv";

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

            string userId = _userManager.GetUserId(User);
             
            List<Entry> entries = await _context.Entries
               .Where(ent => ent.UserID == userId)
               .Include(ent => ent.Enrollments)
                   .ThenInclude(enr => enr.Student)
               .Include(ent => ent.SubjectAssignments)
                   .ThenInclude(sa => sa.Subject)
               .AsNoTracking()
               .ToListAsync();

            var stringWriter = new StringWriter();
            var csvWriter = new CsvWriter(stringWriter);

            using (csvWriter)
            {
                var columnHeader = new
                {
                    Title = "Title",
                    Date = "Date",
                    Description = "Description",
                    Minutes = "Minutes Spent",
                    Students = "Students",
                    Subjects = "Subjects"
                };

                csvWriter.WriteRecord(columnHeader);
                csvWriter.NextRecord();

                foreach (Entry entry in entries)
                {
                    List<string> studentNamesList = entry
                        .Enrollments
                        .Select(enr => enr.Student.Name)
                        .ToList();
                        
                    var record = new
                    {
                        Date = entry.Date.ToShortDateString(),
                        Title = entry.Title,
                        Description = entry.Description,
                        Minutes = entry.MinutesSpent,
                        Students = GetStudentNamesString(studentNamesList),
                        Subjects = GetSubjectNames(entry)
                    };

                    csvWriter.WriteRecord(record);
                    csvWriter.NextRecord();
                }
            }

            var bytes = Encoding.UTF8.GetBytes(stringWriter.ToString());
            return File(bytes, "application/octet-stream", FileName);
        }
    }
}
