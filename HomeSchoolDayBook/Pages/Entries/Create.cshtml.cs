using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;

namespace HomeSchoolDayBook.Pages.Entries
{
    public class CreateModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public EntryVM EntryVM { get; set; }

        public string DidNotSaveMessage { get; set; }

        public CreateModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult OnGet()
        {
            string userId = _userManager.GetUserId(User);

            EntryVM = new EntryVM(_context, userId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string[] selectedSubjects, string[] selectedStudents)
        {
            string userId = _userManager.GetUserId(User);

            Entry newEntry = new Entry
            {
                UserID = userId,
                SubjectAssignments = new List<SubjectAssignment>()
            };

            foreach (Subject subject in _context.Subjects.Where(su =>su.UserID == userId))
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

            foreach (Student student in _context.Students.Where(st => st.UserID == userId))
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

            EntryVM = new EntryVM(newEntry, _context, userId);

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