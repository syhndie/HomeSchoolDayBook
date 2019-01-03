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
    public class CreateModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public EntryVM EntryVM { get; set; }

        public string DidNotSaveMessage { get; set; }

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
            Entry newEntry = new Entry { SubjectAssignments = new List<SubjectAssignment>() };

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

            EntryVM = new EntryVM(newEntry, _context);

            bool modelDidUpdate = await TryUpdateModelAsync<EntryVM>(EntryVM);

            EntryVM.Entry.MinutesSpent = EntryVM.EnteredTotalMinutes;

            if (ModelState.IsValid && modelDidUpdate)
            {
                _context.Entries.Add(EntryVM.Entry);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DidNotSaveMessage = "New Entry did not save correctly. Please try again.";

            return Page();
        }
    }
}