using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using static HomeSchoolDayBook.Helpers.Helpers;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class EntriesInBriefModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string ReportStudentNames { get; set; }

        public List<EntriesInBriefVM> EntriesInBriefVMs { get; set; }

        public EntriesInBriefModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
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

            var studentNamesList = _context.Students
                .Where(st => st.UserID == userId)
                .Where(st => studentIntIDs.Contains(st.ID))
                .OrderBy(st => st.Name)
                .Select(st => st.Name)
                .ToList();

            ReportStudentNames = GetStringFromList(studentNamesList);

            EntriesInBriefVMs = _context.Entries
                .Where(ent => ent.UserID == userId)
                .Where(ent => StartDate <= ent.Date)
                .Where(ent => ent.Date <= EndDate)
                .Include(ent => ent.Enrollments)
                    .ThenInclude(enr => enr.Student)
                .Include(ent => ent.SubjectAssignments)
                    .ThenInclude(sa => sa.Subject)
                .Where(ent => ent.Enrollments.Any(enr => studentIntIDs.Contains(enr.StudentID)))
                .OrderBy(ent => ent.Date)
                    .ThenBy(ent => ent.Title)
                .Select(ent => new EntriesInBriefVM
                    (
                        ent.Date, 
                        ent.Title, 
                        ent.Enrollments.Select(enr => enr.Student.Name).OrderBy(name => name).ToList(), 
                        ent.SubjectAssignments.Select(sa => sa.Subject.Name).OrderBy(name => name).ToList())
                    )
                .ToList();
        }
    }
}