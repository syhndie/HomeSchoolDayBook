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
        
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Title { get; set; }
        public int? MinutesSpent { get; set; }
        public string Description { get; set; }

        [Display(Name = "Time Spent" )]
        public int? EnteredHours { get; set; }

        public int? EnteredMinutes { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Entry entry = await _context.Entries.FindAsync(id);

            Date = entry.Date;
            Title = entry.Title;            
            Description = entry.Description;

            EnteredHours = entry.ComputedHours;
            EnteredMinutes = entry.ComputedMinutes;

            if (entry == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var entryToUpdate = await _context.Entries.FindAsync(id);

            entryToUpdate.MinutesSpent = (EnteredHours * 60) + EnteredMinutes;

            if (await TryUpdateModelAsync<Entry>(entryToUpdate))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
