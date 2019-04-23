﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;

namespace HomeSchoolDayBook.Pages.Entries
{
    public class EditModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public EntryVM EntryVM { get; set; }

        public EditModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
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
                .Include(ent => ent.Grades)
                .Where(ent => ent.UserID == userId)
                .Where(ent => ent.ID == id)
                .FirstOrDefaultAsync();            

            if (entry == null)
            {
                DangerMessage = "Entry not found. The Entry you selected is no longer in the database.";

                return RedirectToPage("./Index");
            }

            EntryVM = new EntryVM(entry, _context, userId);

            Dictionary<string, string> gradesDictionary = new Dictionary<string, string>();
            foreach (Grade grade in entry.Grades)
            {
                gradesDictionary.Add($"earned-student-{grade.StudentID}-subject-{grade.SubjectID}", grade.PointsEarned.ToString());
                gradesDictionary.Add($"available-student-{grade.StudentID}-subject-{grade.SubjectID}", grade.PointsAvailable.ToString());
            }

            EntryVM.GradesJSON = $"{JsonConvert.SerializeObject(gradesDictionary)}";
            // "earned-student-" + studentCheckboxes[i].value + "-subject-" + subjectCheckboxes[j].value
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
                .Include(ent => ent.Grades)
                .Where(ent => ent.UserID == userID)
                .Where(ent => ent.ID == id)
                .FirstOrDefaultAsync();

            if (editedEntry == null)
            {
                DangerMessage = "Entry not found. The Entry you selected is no longer in the database.";

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

            DangerMessage = "Changes did not save correctly. Please try again.";

            return RedirectToPage();
        }
    }
}
