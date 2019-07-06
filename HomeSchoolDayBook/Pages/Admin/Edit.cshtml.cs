using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Data;
using Microsoft.AspNetCore.Authorization;
using static HomeSchoolDayBook.Helpers.Constants;
using Microsoft.EntityFrameworkCore;


namespace HomeSchoolDayBook.Pages.Admin
{
    [Authorize(Roles = AdminRoleName)]
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public HomeSchoolDayBookUser UserToEdit { get; set; }

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string userToEditId)
        {
            UserToEdit = await _context.Users.Where(u => u.Id == userToEditId).FirstOrDefaultAsync();

            if (UserToEdit == null)
            {
                DangerMessage = "User not found.";

                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userToEditId)
        {
            return Page();          
        }
    }
}