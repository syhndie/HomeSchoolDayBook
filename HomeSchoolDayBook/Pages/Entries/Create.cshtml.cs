using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Models.ViewModels;

namespace HomeSchoolDayBook.Pages.Entries
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public EntryVM EntryVM { get; set; }

        public CreateModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            EntryVM = new EntryVM(_context);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string[] selectedSubjects, string[] selectedStudents)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newEntry = EntryVM.Entry;

            newEntry.MinutesSpent = EntryVM.EnteredTotalMinutes;

            newEntry.SubjectAssignments = new List<SubjectAssignment>();

            foreach (Subject subject in _context.Subjects)
            {
                if (selectedSubjects.Contains(subject.ID.ToString()))
                {
                    newEntry.SubjectAssignments.Add(new SubjectAssignment
                    {
                        SubjectID = subject.ID,
                        EntryID = newEntry.ID
                    });
                }
            }

            newEntry.Enrollments = new List<Enrollment>();

            foreach (Student student in _context.Students)
            {
                if (selectedStudents.Contains(student.ID.ToString()))
                {
                    newEntry.Enrollments.Add(new Enrollment
                    {
                        StudentID = student.ID,
                        EntryID = newEntry.ID
                    });
                }
            }

            if (await TryUpdateModelAsync<Entry>(newEntry, "entryvm"))
            {
                _context.Entries.Add(newEntry);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return null;
        }       
    }
}