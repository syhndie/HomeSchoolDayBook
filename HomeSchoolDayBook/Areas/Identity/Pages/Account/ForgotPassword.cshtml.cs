using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<HomeSchoolDayBookUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        private const int maxAllowedEmails = 5;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./Login");
                }
                
                if (user.ForgotPasswordEmailsCount < maxAllowedEmails)
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "Reset Password",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    user.ForgotPasswordEmailsCount++;
                    await _userManager.UpdateAsync(user);

                    //Don't reveal that the user does exist
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
            }

            return RedirectToPage("./Login");
        }
    }
}
