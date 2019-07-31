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
using System.ComponentModel.DataAnnotations;


namespace HomeSchoolDayBook.Pages.Admin
{
    [Authorize(Roles = AdminRoleName)]
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        /// <summary>datetime account was created</summary>
        [DataType(DataType.DateTime)]
        public DateTime AccountCreatedTimeStamp { get; set; }

        /// <summary>number of emails sent to confirm email address, is reset to zero on confirmation</summary>
        public int EmailConfirmsCount { get; set; }

        /// <summary>number of emails sent to change password, is reset to zero on password change</summary>
        public int ForgotPasswordEmailCount { get; set; }

        ///<summary>this is not null when a user has requested to change email, but has not yet confirmed the new email address</summary>
        [DataType(DataType.EmailAddress)]
        public string PendingEmail { get; set; }

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

            Email = userToEdit.Email;
            AccountCreatedTimeStamp = userToEdit.AccountCreatedTimeStamp;
            EmailConfirmed = userToEdit.EmailConfirmed;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userToEditId)
        {
            return Page();          
        }
    }
}