using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using static HomeSchoolDayBook.Helpers.Helpers;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class EntriesInFullModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string ReportStudentNames { get; set; }

        public List<EntriesInFullVM> EntriesInFullVMs { get; set; }

        public EntriesInFullModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public void OnGet(string start, string end, string studentIDs)
        {
            string userId = _userManager.GetUserId(User);

            StartDate = Convert.ToDateTime(start);

            EndDate = Convert.ToDateTime(end);

            List<int> studentIntIDs = studentIDs is null || studentIDs == ""
                ? new List<int>() :
                studentIDs.Split(',')
                .Select(Int32.Parse)
                .ToList();

            List<string> studentNamesList = _context.Students
                .Where(st => st.UserID == userId)
                .Where(st => studentIntIDs.Contains(st.ID))
                .OrderBy(st => st.Name)
                .Select(st => st.Name)
                .ToList();

            ReportStudentNames = GetStringFromList(studentNamesList);

            List<Entry> entries = _context.Entries
                .Where(ent => ent.UserID == userId)
                .Where(ent => StartDate <= ent.Date)
                .Where(ent => ent.Date <= EndDate)
                .Include(ent => ent.Enrollments)
                    .ThenInclude(enr => enr.Student)
                .Include(ent => ent.SubjectAssignments)
                    .ThenInclude(sa => sa.Subject)
                .Include(ent => ent.Grades)
                .Where(ent => ent.Enrollments.Any(enr => studentIntIDs.Contains(enr.StudentID)))
                .OrderBy(ent => ent.Date)
                    .ThenBy(ent => ent.Title)
                .ToList();

            EntriesInFullVMs = new List<EntriesInFullVM>();

            foreach (Entry entry in entries)
            {
                List<string> studentNames = entry.Enrollments.Select(enr => enr.Student.Name).ToList();
                List<string> subjectNames = entry.SubjectAssignments.Select(sa => sa.Subject.Name).ToList();

                EntriesInFullVMs.Add(new EntriesInFullVM
                    (
                        entry.Date,
                        entry.Title, 
                        entry.Description, 
                        GetStringFromList(studentNames), 
                        GetStringFromList(subjectNames),
                        entry.ComputedTimeSpent,
                        entry.Grades.ToList())
                    );
            }
        }
    }
}
 