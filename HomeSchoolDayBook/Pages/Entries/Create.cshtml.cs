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
using static HomeSchoolDayBook.Helpers.Helpers;

namespace HomeSchoolDayBook.Pages.Entries
{
    public class CreateModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public EntryCreateEditVM EntryCreateEditVM { get; set; }

        public CreateModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult OnGet()
        {
            string userId = _userManager.GetUserId(User);

            EntryCreateEditVM = new EntryCreateEditVM(_context, userId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string[] selectedSubjects, string[] selectedStudents)
        {
            var formData = Request.Form;

            string userId = _userManager.GetUserId(User);

            EntryCreateEditVM = new EntryCreateEditVM();

            bool modelDidUpdate = await TryUpdateModelAsync<EntryCreateEditVM>(EntryCreateEditVM);

            if (!modelDidUpdate)
            {
                DangerMessage = "Changes did not save correctly. Please try again.";
                return RedirectToPage();
            }

            Entry newEntry = new Entry
            {
                UserID = userId,
                Title = EntryCreateEditVM.Title,
                Date = EntryCreateEditVM.Date,
                Description = EntryCreateEditVM.Description
            };

            if (EntryCreateEditVM.Hours == null && EntryCreateEditVM.Minutes == null)
            {
                newEntry.MinutesSpent = null;
            }
            else newEntry.MinutesSpent = ((EntryCreateEditVM.Hours ?? 0) * 60) + (EntryCreateEditVM.Minutes ?? 0);

            newEntry.SubjectAssignments = new List<SubjectAssignment>();

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

            List<Grade> grades = GetGradesFromFormData(formData, newEntry, userId, out bool allGradesValid);

            newEntry.Grades = grades;
            
            _context.Entries.Add(newEntry);

            await _context.SaveChangesAsync();

            if (!allGradesValid) DangerMessage = "At least one grade was not entered correctly and was not saved.";

            return RedirectToPage("./Index");
        }
    }
}