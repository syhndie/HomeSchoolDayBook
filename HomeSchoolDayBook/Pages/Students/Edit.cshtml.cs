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

namespace HomeSchoolDayBook.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;

        public Student Student { get; set; }

        [TempData]
        public string NotFoundMessage {get; set;}

        public string DidNotSaveMessage { get; set; }

        public EditModel(HomeSchoolDayBook.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Student = await _context.Students.FirstOrDefaultAsync(m => m.ID == id);

            if (Student == null)
            {
                NotFoundMessage = "Student not found. The Student you selected is no longer in the database.";

                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            Student = await _context.Students.FirstOrDefaultAsync(m => m.ID == id);

            if (Student == null)
            {
                NotFoundMessage = "Student not found. The Student you selected is not longer in the database.";

                return RedirectToPage("./Index");
            }

            bool modelDidUpdate = await TryUpdateModelAsync<Student>(Student);

            if (ModelState.IsValid && modelDidUpdate)
            {
                List<string> otherUsedNames = _context.Students.Where(s => s.ID != id).Select(s=> s.Name).ToList();

                if (otherUsedNames.Contains(Student.Name))
                {
                    DidNotSaveMessage = "This Student is already in the database.";

                    return Page();
                }

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            DidNotSaveMessage = "Changes did not save correctly. Please try again.";

            return Page();
        }
    }
}