using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Models.ViewModels;
using HomeSchoolDayBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Data;
using Microsoft.AspNetCore.Authorization;
using static HomeSchoolDayBook.Helpers.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;

namespace HomeSchoolDayBook.Pages.Admin
{
    [Authorize(Roles = AdminRoleName)]
    public class EditModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        private readonly IEmailSender _emailSender;

        private readonly ILogger<EditModel> _logger;

        public EditUser EditUser { get; set; }
        
        public string UserToEditID { get; set; }

        public EditModel(UserManager<HomeSchoolDayBookUser> userManager, ApplicationDbContext context, IEmailSender emailSender, ILogger<EditModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string userToEditId)
        {
            UserToEditID = userToEditId;

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
                userToEdit.PendingEmail,
                userToEdit.NewEmailConfirmsCount
                );

            return Page();
        }

        public async Task<IActionResult> OnPostResendEmailAsync(string userToEditID)
        {
            UserToEditID = userToEditID;

            HomeSchoolDayBookUser userToEdit = await _userManager.Users.Where(u => u.Id == userToEditID).SingleOrDefaultAsync();

            if (userToEdit == null)
            {
                DangerMessage = "User not found.";

                return RedirectToPage("./Index");
            }

            if (await _userManager.IsEmailConfirmedAsync(userToEdit))
            {
                DangerMessage = "User email already confirmed.";

                return RedirectToPage("./Index");
            }

            string code = await _userManager.GenerateEmailConfirmationTokenAsync(userToEdit);

            string callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userToEdit.Id, code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                userToEdit.Email,
                "Confirm your HomeSchoolDayBook account email address",
                $"Please confirm the email you provided to HomeSchoolDayBook by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            SuccessMessage = "Verification email sent.";

            return RedirectToPage("./Index");
        }
      
public async Task<IActionResult> OnPostEditAsync(string userToEditId)
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

                    userToEdit.EmailConfirmed = EditUser.EmailConfirmed;
                    userToEdit.EmailConfirmsCount = EditUser.EmailConfirmsCount;
                    userToEdit.ForgotPasswordEmailsCount = EditUser.ForgotPasswordEmailCount;
                    userToEdit.PendingEmail = EditUser.PendingEmail;
                    userToEdit.NewEmailConfirmsCount = EditUser.NewEmailConfirmsCount;

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
        }
    }
}