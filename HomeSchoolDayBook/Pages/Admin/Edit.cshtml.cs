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
using Microsoft.Extensions.Logging;


namespace HomeSchoolDayBook.Pages.Admin
{
    [Authorize(Roles = AdminRoleName)]
    public class EditModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        private readonly ILogger<EditModel> _logger;

        public EditUser EditUser { get; set; }

        public EditModel(UserManager<HomeSchoolDayBookUser> userManager, ApplicationDbContext context, ILogger<EditModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string userToEditId)
        {
            HomeSchoolDayBookUser userToEdit = await _userManager.Users.Where(u => u.Id == userToEditId).SingleOrDefaultAsync();
            
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
            if (userToEditId == null)
            {
                DangerMessage = "User not found.";
                return RedirectToPage("./Index");
            }

            EditUser = new EditUser();

            bool modelDidUpdate = await TryUpdateModelAsync<EditUser>(EditUser, "edituser");

            if (!modelDidUpdate)
            {
                DangerMessage = "Changes did not save correctly. Please try again.";
                return RedirectToPage();
            }

            HomeSchoolDayBookUser userToEdit = await _userManager.Users.Where(u => u.Id == userToEditId).SingleOrDefaultAsync();
            
            if (userToEdit == null)
            {
                DangerMessage = "User not found.";

                return RedirectToPage("./Index");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    bool emailKosher = true;

                    if (EditUser.Email != userToEdit.Email)
                    {
                        string changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(userToEdit, EditUser.Email);
                        IdentityResult changeEmailResult = await _userManager.ChangeEmailAsync(userToEdit, EditUser.Email, changeEmailToken);
                        IdentityResult changeNameResult = await _userManager.SetUserNameAsync(userToEdit, EditUser.Email);

                        if (!changeEmailResult.Succeeded || !changeNameResult.Succeeded) emailKosher = false;
                    }
                     
                    userToEdit.EmailConfirmsCount = EditUser.EmailConfirmsCount;
                    userToEdit.ForgotPasswordEmailsCount = EditUser.ForgotPasswordEmailCount;
                    userToEdit.PendingEmail = EditUser.PendingEmail;

                    IdentityResult updateUserResult = await _userManager.UpdateAsync(userToEdit);
                    
                    if (emailKosher && updateUserResult.Succeeded)
                    {
                        transaction.Commit();
                        _logger.LogInformation("User information modified by admin.");
                        return RedirectToPage("./Index");
                    }
                    throw new Exception($"emailKosher = {emailKosher}, updateUserResult.Succeeded = {updateUserResult.Succeeded}");
                }
                catch (Exception ex)
                {
                    DangerMessage = $"An error occurred when modifying user data: {ex.Message}";
                    return RedirectToPage("./Index");
                }                
            }
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var result = await _userManager.ConfirmEmailAsync(user, code);             
        }
    }
}