using System;
using System.Collections.Generic;
using System.Linq;
using HomeSchoolDayBook.Data;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Models;
using Microsoft.EntityFrameworkCore;
using static HomeSchoolDayBook.Helpers.Helpers;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class TimeSpentPerSubjectModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public List<TimeSpentPerSubjectVM> TimeSpentPerSubjectVMs  { get; set; }

        public TimeSpentPerSubjectModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public void OnGet(string start, string end, string studentIDs)
        {
            string userId = _userManager.GetUserId(User);

            StartDate = Convert.ToDateTime(start);

            EndDate = Convert.ToDateTime(end);

            if (studentIDs == null || studentIDs == "") TimeSpentPerSubjectVMs = new List<TimeSpentPerSubjectVM>();
            else
            {
                List<int> studentIDList = studentIDs.Split(',')
                    .Select(Int32.Parse)
                    .ToList();

                List<Entry> entries = _context.Entries
                    .Include(ent => ent.Enrollments)
                        .ThenInclude(enr => enr.Student)
                    .Include(ent => ent.SubjectAssignments)
                        .ThenInclude(sa => sa.Subject)
                    .Where(ent => ent.UserID == userId)
                    .Where(ent => ent.Date <= EndDate)
                    .Where(ent => StartDate <= ent.Date)
                    .Where(ent => ent.Enrollments.Any(enr => studentIDList.Contains(enr.StudentID)))
                    .Where(ent => ent.MinutesSpent > 0)
                    .ToList();

                TimeSpentPerSubjectVMs = entries
                    .SelectMany(ent => ent.Enrollments
                        .Select(enr => enr.Student)
                            .SelectMany(st => ent.SubjectAssignments
                                .Select(sa => sa.Subject)
                            .Select(su => new { st, su, ent.MinutesSpent})))
                    .Where(a => studentIDList.Contains(a.st.ID))
                    .ToList()
                    .GroupBy(a => new { a.st, a.su})
                    .ToList()
                    .Select(x => new TimeSpentPerSubjectVM(x.Key.st.Name, x.Key.su.Name, (int)x.Sum(y => y.MinutesSpent)))
                    .OrderBy(tsps => tsps.StudentName)
                    .ThenBy(tsps => tsps.SubjectName)
                    .ToList()
                    ;
            }
        }
    }
}