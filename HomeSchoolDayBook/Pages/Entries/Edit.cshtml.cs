using System;
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

namespace HomeSchoolDayBook.Pages.Entries
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public EditModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
            
        }
        
        public EntryVM EntryVM { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Entry entry = await _context.Entries
                .Include(ent => ent.SubjectAssignments)
                    .ThenInclude(sa => sa.Subject)
                .Include(ent => ent.Enrollments)
                    .ThenInclude(enr => enr.Student)
                .AsNoTracking()
                .FirstOrDefaultAsync(ent => ent.ID == id);

            EntryVM = new EntryVM(entry, _context);

            if (entry == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedSubjects, string[] selectedStudents)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var entryToUpdate = await _context.Entries
                .Include(ent => ent.SubjectAssignments)
                    .ThenInclude(sa => sa.Subject)
                .Include(ent => ent.Enrollments)
                    .ThenInclude(enr => enr.Student)
                .FirstOrDefaultAsync(ent => ent.ID == id);

            entryToUpdate.MinutesSpent = EntryVM.EnteredTotalMinutes;

            entryToUpdate.SubjectAssignments = new List<SubjectAssignment>();
            
            foreach (Subject subject in _context.Subjects)
            {
                if (selectedSubjects.Contains(subject.ID.ToString()))
                {
                    entryToUpdate.SubjectAssignments.Add(new SubjectAssignment
                    {
                        SubjectID = subject.ID,
                        EntryID = entryToUpdate.ID
                    });
                }
            }

            entryToUpdate.Enrollments = new List<Enrollment>();

            foreach (Student student in _context.Students)
            {
                if (selectedStudents.Contains(student.ID.ToString()))
                {
                    entryToUpdate.Enrollments.Add(new Enrollment
                    {
                        StudentID = student.ID,
                        EntryID = entryToUpdate.ID
                    });
                }
            }
            
            if (await TryUpdateModelAsync<Entry>(entryToUpdate, "entryvm"))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
