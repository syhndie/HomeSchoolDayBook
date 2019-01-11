﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HomeSchoolDayBook.Pages.Entries
{
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public EntryVM EntryVM { get; set; }

        [TempData]
        public string NotFoundMessage { get; set; }

        public string DidNotSaveMessage { get; set; }

        public EditModel(HomeSchoolDayBook.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Entry entry = await _context.Entries
                .Include(ent => ent.SubjectAssignments)
                    .ThenInclude(sa => sa.Subject)
                .Include(ent => ent.Enrollments)
                    .ThenInclude(enr => enr.Student)
                .AsNoTracking()
                .Where(ent => ent.UserID == userId)
                .Where(ent => ent.ID == id)
                .FirstOrDefaultAsync();            

            if (entry == null)
            {
                NotFoundMessage = "Entry not found. The Entry you selected is no longer in the database.";

                return RedirectToPage("./Index");
            }

            EntryVM = new EntryVM(entry, _context, userId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedSubjects, string[] selectedStudents)
        {
            string userID = _userManager.GetUserId(User);

            Entry editedEntry = await _context.Entries
                .Include(ent => ent.SubjectAssignments)
                    .ThenInclude(sa => sa.Subject)
                .Include(ent => ent.Enrollments)
                    .ThenInclude(enr => enr.Student)
                .Where(ent => ent.UserID == userID)
                .Where(ent => ent.ID == id)
                .FirstOrDefaultAsync();

            if (editedEntry == null)
            {
                NotFoundMessage = "Entry not found. The Entry you selected is no longer in the database.";

                return RedirectToPage("./Index");
            }

            editedEntry.SubjectAssignments = new List<SubjectAssignment>();
            
            foreach (Subject subject in _context.Subjects.Where(su => su.UserID == userID))
            {
                if (selectedSubjects.Contains(subject.ID.ToString()))
                {
                    editedEntry.SubjectAssignments.Add(new SubjectAssignment
                    {
                        SubjectID = subject.ID,
                        EntryID = editedEntry.ID
                    });
                }
            }

            editedEntry.Enrollments = new List<Enrollment>();

            foreach (Student student in _context.Students.Where(st => st.UserID == userID))
            {
                if (selectedStudents.Contains(student.ID.ToString()))
                {
                    editedEntry.Enrollments.Add(new Enrollment
                    {
                        StudentID = student.ID,
                        EntryID = editedEntry.ID
                    });
                }
            }

            EntryVM = new EntryVM(editedEntry, _context, userID);

            bool modelDidUpdate = await TryUpdateModelAsync<EntryVM>(EntryVM);

            EntryVM.Entry.MinutesSpent = EntryVM.EnteredTotalMinutes;

            if (ModelState.IsValid && modelDidUpdate)
            {
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DidNotSaveMessage = "Changes did not save correctly. Please try again.";

            return Page();
        }
    }
}
