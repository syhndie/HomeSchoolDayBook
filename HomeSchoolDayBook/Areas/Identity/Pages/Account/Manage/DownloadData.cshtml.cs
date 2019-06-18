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
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Areas.Identity.Pages.Account.Manage
{
    public class DownloadDataModel : BasePageModel
    {
        private readonly static string DateStamp = DateTime.Now.ToString("yyyyMMdd");
        private readonly string EntriesFileName = $"HSDBEntries{DateStamp}.csv";
        private readonly string GradesFileName = $"HSDBGrades{DateStamp}.csv";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;
        private readonly ILogger<DownloadDataModel> _logger;

        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "From")]
        public DateTime FromDate { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "To")]
        public DateTime ToDate { get; set; }

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
            FromDate = DateTime.Today.AddMonths(-1);
            ToDate = DateTime.Today;
        }

        public async Task<IActionResult> OnPostEntriesAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                DangerMessage = "Unable to load user.";
                return RedirectToPage();
            }

            _logger.LogInformation("User with ID '{UserId}' asked for data download.", _userManager.GetUserId(User));

            string userId = _userManager.GetUserId(User);

            DateTime startDate = FromDate <= ToDate ? FromDate : ToDate;
            DateTime endDate = startDate == FromDate ? ToDate : FromDate;
             
            List<Entry> entries = await _context.Entries
               .Where(ent => ent.UserID == userId)
               .Where(ent => startDate <= ent.Date)
               .Where(ent => ent.Date <= endDate)
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
                    ID = "Entry ID",
                    Date = "Date",
                    Title = "Title",
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

                    List<string> subjectNamesList = entry
                        .SubjectAssignments
                        .Select(sa => sa.Subject.Name)
                        .ToList();
                        
                    var record = new
                    {
                        ID = entry.ID,
                        Date = entry.Date.ToShortDateString(),
                        Title = entry.Title,
                        Description = entry.Description,
                        Minutes = entry.MinutesSpent,
                        Students = GetStringFromList(studentNamesList),
                        Subjects = GetStringFromList(subjectNamesList)
                    };

                    csvWriter.WriteRecord(record);
                    csvWriter.NextRecord();
                }
            }

            var bytes = Encoding.UTF8.GetBytes(stringWriter.ToString());
            return File(bytes, "application/octet-stream", EntriesFileName);
        }

        public async Task<IActionResult> OnPostGradesAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                DangerMessage = "Unable to load user.";
                return RedirectToPage();
            }

            _logger.LogInformation("User with ID '{UserId}' asked for data download.", _userManager.GetUserId(User));

            string userId = _userManager.GetUserId(User);

            DateTime startDate = FromDate <= ToDate ? FromDate : ToDate;
            DateTime endDate = startDate == FromDate ? ToDate : FromDate;

            List<Grade> grades = await _context.Grades
               .Where(gr => gr.UserID == userId)
               .Where(gr => startDate <= gr.Entry.Date)
               .Where(gr => gr.Entry.Date <= endDate)
               .Include(gr => gr.Student)
               .Include(gr => gr.Subject)
               .AsNoTracking()
               .ToListAsync();

            var stringWriter = new StringWriter();
            var csvWriter = new CsvWriter(stringWriter);

            using (csvWriter)
            {
                var columnHeader = new
                {
                    ID = "Entry ID",
                    StudentName = "Student",
                    SubjectName = "Subject",
                    PointsEarned = "Points Earned",
                    PointsAvailable = "Points Available"
                };

                csvWriter.WriteRecord(columnHeader);
                csvWriter.NextRecord();

                foreach (Grade grade in grades)
                {
                    var record = new
                    {
                        ID = grade.EntryID,
                        StudentName = grade.Student.Name,
                        SubjectName = grade.Subject.Name,
                        PointsEarned = grade.PointsEarned,
                        PointsAvailable = grade.PointsAvailable
                    };

                    csvWriter.WriteRecord(record);
                    csvWriter.NextRecord();
                }
            }

            var bytes = Encoding.UTF8.GetBytes(stringWriter.ToString());
            return File(bytes, "application/octet-stream", GradesFileName);
        }
    }
}
