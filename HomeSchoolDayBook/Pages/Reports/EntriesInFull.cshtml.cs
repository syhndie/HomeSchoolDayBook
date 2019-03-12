using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using static HomeSchoolDayBook.Helpers.Helpers;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class EntriesInFullModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public EntriesReportVM EntriesReportVM { get; set; }

        [Display(Name = "Students")]
        public Dictionary<int, string> StudentNameLookup { get; set; }

        [Display(Name = "Subjects")]
        public Dictionary<int, string> SubjectNameLookup { get; set; }
        
        public EntriesInFullModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
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
                string studentNames = GetStudentNames(entry);

                StudentNameLookup.Add(entry.ID, studentNames);
            }

            SubjectNameLookup = new Dictionary<int, string>();

            foreach (Entry entry in EntriesReportVM.Entries)
            {
                string subjectNames = GetSubjectNames(entry);

                SubjectNameLookup.Add(entry.ID, subjectNames);
            }
        }
    }
}