﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;

namespace HomeSchoolDayBook.Pages.Entries
{
    public class DeleteModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public Entry Entry { get; set; }

        public DeleteModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Entry = await _context.Entries
                .Where(ent => ent.UserID == userId)
                .Where(ent => ent.ID == id)
                .Include(ent => ent.Enrollments)
                    .ThenInclude(enr => enr.Student)
                .Include(ent => ent.SubjectAssignments)
                    .ThenInclude(sa => sa.Subject)
                .Include(ent => ent.Grades)
                    .ThenInclude(gr => gr.Student)
                .Include(ent => ent.Grades)
                    .ThenInclude(gr => gr.Subject)
                .FirstOrDefaultAsync();

            if (Entry == null)
            {
                DangerMessage = "Entry not found.";

                return RedirectToPage("./Index");
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            string userId = _userManager.GetUserId(User);

            Entry = await _context.Entries
                .Where(ent => ent.UserID == userId)
                .Where(ent => ent.ID == id)
                .FirstOrDefaultAsync();

            if (Entry == null)
            {
                DangerMessage = "Entry not found.";

                return RedirectToPage("./Index");
            }

            _context.Remove(Entry);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
