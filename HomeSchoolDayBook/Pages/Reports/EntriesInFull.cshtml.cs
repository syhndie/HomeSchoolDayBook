using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Data;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class EntriesInFullModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ApplicationDbContext _context;

        public EntriesReportVM EntriesReportVM { get; set; }

        [Display(Name = "Students")]
        public Dictionary<int, string> StudentNameLookup { get; set; }

        [Display(Name = "Subjects")]
        public Dictionary<int, string> SubjectNameLookup { get; set; }
        
        public EntriesInFullModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public void OnGet(string start, string end, string studentIDs)
        {
            string userId = _userManager.GetUserId(User);

            EntriesReportVM = new EntriesReportVM(start, end, studentIDs, _context, userId);

            StudentNameLookup = new Dictionary<int, string>();

            foreach (Entry entry in EntriesReportVM.Entries)
            {
                List<Enrollment> enrollments = entry.Enrollments.OrderBy(enr => enr.Student.Name).ToList();

                string studentNames = "";

                for (int i = 0; i < entry.Enrollments.Count(); i++)
                {
                    if (i == 0)
                    {
                        studentNames = enrollments[i].Student.Name;
                    }
                    else if (i == entry.Enrollments.Count() - 1)
                    {
                        studentNames = $"{studentNames}, and {enrollments[i].Student.Name}";
                    }
                    else
                    {
                        studentNames = $"{studentNames}, {enrollments[i].Student.Name}";
                    }
                }

                StudentNameLookup.Add(entry.ID, studentNames);
            }

            SubjectNameLookup = new Dictionary<int, string>();

            foreach (Entry entry in EntriesReportVM.Entries)
            {
                List<SubjectAssignment> subjectAssignments = entry.SubjectAssignments.OrderBy(sa => sa.Subject.Name).ToList();

                string subjectNames = "";

                for (int i = 0; i < entry.SubjectAssignments.Count(); i++)
                {
                    if (i == 0)
                    {
                        subjectNames = subjectAssignments[i].Subject.Name;
                    }
                    else if (i == entry.SubjectAssignments.Count() - 1)
                    {
                        subjectNames = $"{subjectNames}, and {subjectAssignments[i].Subject.Name}";
                    }
                    else
                    {
                        subjectNames = $"{subjectNames}, {subjectAssignments[i].Subject.Name}";
                    }
                }

                SubjectNameLookup.Add(entry.ID, subjectNames);
            }
        }
    }
}