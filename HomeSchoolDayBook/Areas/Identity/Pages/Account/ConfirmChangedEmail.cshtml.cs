using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using HomeSchoolDayBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HomeSchoolDayBook.Data;
using Microsoft.Extensions.Logging;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmChangedEmailModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ConfirmChangedEmailModel> _logger;

        public ConfirmChangedEmailModel(
            UserManager<HomeSchoolDayBookUser> userManager, 
            ApplicationDbContext context, 
            ILogger<ConfirmChangedEmailModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public string ErrorMessage { get; set; }
        
        public async Task<IActionResult> OnGetAsync(string userId, string changeEmailToken, string newEmail)
        {
            if (userId == null || changeEmailToken == null || newEmail == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                DangerMessage = "Unable to load user.";
                return RedirectToPage("/Index");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var oldEmail = await _userManager.GetEmailAsync(user);
                    if (newEmail != oldEmail)
                    {
                        IdentityResult changeEmailresult = await _userManager.ChangeEmailAsync(user, newEmail, changeEmailToken);
                        IdentityResult changeNameResult = await _userManager.SetUserNameAsync(user, newEmail);
                        user.PendingEmail = null;
                        await _userManager.UpdateAsync(user);
                    
                        if (changeEmailresult.Succeeded && changeNameResult.Succeeded)
                        {                          
                            transaction.Commit();
                            _logger.LogInformation("User confirmed new email and changed email address");                            
                        }
                    }

                    return Page();
                }
                catch (Exception)
                {
                    DangerMessage = "An error occurred when updating your email address.";
                    return RedirectToPage();
                }
            }
        }
    }
}