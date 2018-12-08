using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models.ViewModels;

using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        [DataType(DataType.Date)]
        [Required]
        [Display(Name ="Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name ="End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Students")]
        public List<CheckBoxVM> StudentCheckBoxes { get; set; }

        [Display(Name ="Choose a Report")]
        public List<CheckBoxVM> ReportRadioButtons { get; set; }

        public IndexModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            StartDate = DateTime.Today.AddMonths(-1);

            EndDate = DateTime.Today;

            StudentCheckBoxes = await _context
                .Students
                .Where(st => st.IsActive)
                .OrderBy(st => st.Name)
                .Select(st => new CheckBoxVM(st.ID, st.Name, false))
                .ToListAsync();

            ReportRadioButtons = new List<CheckBoxVM>();
            ReportRadioButtons.Add(new CheckBoxVM(1, "Attendance", false));
            ReportRadioButtons.Add(new CheckBoxVM(2, "Entries in Brief", false));
            ReportRadioButtons.Add(new CheckBoxVM(3, "Entries in Full", false));

            return Page();            
        }

        public IActionResult OnPost(string startDate, string endDate, string[] selectedStudents, string selectedReport)
        {            
            string selectedStudentsAsString = String.Join(',', selectedStudents);
            switch (selectedReport)
            {
                case "1":
                    return RedirectToPage("./Attendance", new { start = startDate, end = endDate, studentIDs = selectedStudentsAsString});
                    
                case "2":
                    return RedirectToPage("./EntriesInBrief");
                    
                case "3":
                    return RedirectToPage("./EntriesInFull");

                default:
                    return Page();
            }         
        }
    }
}