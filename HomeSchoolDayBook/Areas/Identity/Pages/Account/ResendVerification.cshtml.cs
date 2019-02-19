using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using HomeSchoolDayBook.Areas.Identity.Data;


namespace HomeSchoolDayBook.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmModel : PageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ResendEmailConfirmModel(UserManager<HomeSchoolDayBookUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        private const int maxAllowedEmails = 5;

        [TempData]
        public string ConfirmMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return RedirectToPage("./ResendVerification");
                }

                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Don't reveal that user does exist
                    return RedirectToPage("./ResendVerification");
                }

                if (user.EmailConfirmsCount < maxAllowedEmails)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your HomeSchoolDayBook acount email address",
                        $"Please confirm the email you provided to HomeSchoolDayBook by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    user.EmailConfirmsCount++;
                    await _userManager.UpdateAsync(user);

                    ConfirmMessage = "An email has been sent to the address you provided. " +
                    "Please click on the link in that email to verify your address. " +
                    "Once your address has been verified, you may login. " +
                    $"A total of {user.EmailConfirmsCount} verification emails have been sent to this address." +
                    $"A maximum of {maxAllowedEmails} emails are allowed.";

                    return RedirectToPage();
                }
                else
                {
                    ErrorMessage = $"You have exceeded the maximum of {maxAllowedEmails} verification emails. No further verification emails may be sent.";
                    return RedirectToPage();
                }

            }

            ErrorMessage = "An error occurred and the verification email was not sent.";
            return Page();
        }
    }
}