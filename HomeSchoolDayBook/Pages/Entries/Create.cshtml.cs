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
            EntryVM newEntryVM = new EntryVM();

            newEntryVM.Entry = new Entry();
            
            newEntryVM.Entry.SubjectAssignments = new List<SubjectAssignment>();

            foreach (Subject subject in _context.Subjects)
            {
                if (selectedSubjects.Contains(subject.ID.ToString()))
                {
                    newEntryVM.Entry.SubjectAssignments.Add(new SubjectAssignment
                    {
                        SubjectID = subject.ID,
                        EntryID = newEntryVM.Entry.ID
                    });
                }
            }

            newEntryVM.Entry.Enrollments = new List<Enrollment>();

            foreach (Student student in _context.Students)
            {
                if (selectedStudents.Contains(student.ID.ToString()))
                {
                    newEntryVM.Entry.Enrollments.Add(new Enrollment
                    {
                        StudentID = student.ID,
                        EntryID = newEntryVM.Entry.ID
                    });
                }
            }

            bool modelDidUpdate = await TryUpdateModelAsync<EntryVM>(newEntryVM, "entryvm");

            newEntryVM.Entry.MinutesSpent = newEntryVM.EnteredTotalMinutes;

            //if (ModelState.IsValid && modelDidUpdate)
            //{
            //    _context.Entries.Add(newEntryVM.Entry);

            //    await _context.SaveChangesAsync();

            //    return RedirectToPage("./Index");
            //}

            DidNotSaveMessage = "New Entry did not save correctly. Please try again.";

            return Page();
        }
    }
}