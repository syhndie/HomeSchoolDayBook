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

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(string[] selectedSubjects)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyEntry = new Entry();

            emptyEntry.MinutesSpent = EntryVM.EnteredTotalMinutes;

            emptyEntry.SubjectAssignments = new List<SubjectAssignment>();

            foreach (Subject subject in _context.Subjects)
            {
                if (selectedSubjects.Contains(subject.ID.ToString()))
                {
                    emptyEntry.SubjectAssignments.Add(new SubjectAssignment
                    {
                        SubjectID = subject.ID,
                        EntryID = emptyEntry.ID
                    });
                }
            }

            if (await TryUpdateModelAsync<Entry>(emptyEntry, "entryvm"))
            {
                _context.Entries.Add(emptyEntry);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return null;
        }       
    }
}