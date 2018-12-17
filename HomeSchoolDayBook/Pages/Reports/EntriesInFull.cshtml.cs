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

namespace HomeSchoolDayBook.Pages.Reports
{
    public class EntriesInFullModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EntriesReportVM EntriesReportVM { get; set; }

        public Dictionary<int, string> StudentNameLookup { get; set; }
        
        public EntriesInFullModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(string start, string end, string studentIDs)
        {
            EntriesReportVM = new EntriesReportVM(start, end, studentIDs, _context);

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
                        studentNames = $"{studentNames} and {enrollments[i].Student.Name}";
                    }
                    else
                    {
                        studentNames = $"{studentNames}, {enrollments[i].Student.Name}";
                    }
                }

                StudentNameLookup.Add(entry.ID, studentNames);
            }
        }
    }
}