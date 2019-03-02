using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Models;


namespace HomeSchoolDayBook.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ResendEmailConfirmModel(UserManager<HomeSchoolDayBookUser> userManager, IEmailSender emailSender)
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

                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    
                    return RedirectToPage("./Login");
                }

                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Don't reveal that user does exist
                    return RedirectToPage("./Login");
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

                    // Don't reveal that the user does exist
                    return RedirectToPage("./Login");
                }
               
                //Don't reveal that the user does exist
                return RedirectToPage("./Login");

            }

            return RedirectToPage("./Login");
        }
    }
}