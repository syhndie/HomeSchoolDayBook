using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Models.ViewModels;
using HomeSchoolDayBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Data;
using Microsoft.AspNetCore.Authorization;
using static HomeSchoolDayBook.Helpers.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace HomeSchoolDayBook.Pages.Admin
{
    [Authorize(Roles = AdminRoleName)]
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public EditUser EditUser { get; set; }

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string userToEditId)
        {
            HomeSchoolDayBookUser userToEdit = await _context.Users.Where(u => u.Id == userToEditId).SingleOrDefaultAsync();
            
            if (userToEdit == null)
            {
                DangerMessage = "User not found.";

                return RedirectToPage("./Index");
            }

            EditUser = new EditUser
            (
                userToEdit.Email,
                userToEdit.AccountCreatedTimeStamp,
                userToEdit.EmailConfirmed,
                userToEdit.EmailConfirmsCount,
                userToEdit.ForgotPasswordEmailsCount,
                userToEdit.PendingEmail
                );

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userToEditId)
        {
            HomeSchoolDayBookUser userToEdit = await _context.Users.Where(u => u.Id == userToEditId).SingleOrDefaultAsync();

            if (userToEdit == null)
            {
                DangerMessage = "User not found.";

                return RedirectToPage("./Index");
            }
            return Page();          
        }
    }
}