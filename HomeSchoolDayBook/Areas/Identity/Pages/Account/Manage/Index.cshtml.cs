using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HomeSchoolDayBook.Areas.Identity.Data;

namespace HomeSchoolDayBook.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;
        private readonly SignInManager<HomeSchoolDayBookUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            HomeSchoolDayBook.Data.ApplicationDbContext context,
            UserManager<HomeSchoolDayBookUser> userManager,
            SignInManager<HomeSchoolDayBookUser> signInManager,
            IEmailSender emailSender,
            ILogger<IndexModel> logger)
        {
            _context = context; 
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [TempData]
        public string ConfirmMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Email = email
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var newEmail = Input.Email;
            var oldEmail = await _userManager.GetEmailAsync(user);

            if (newEmail != oldEmail)
            {
                user.PendingEmail = newEmail;
                await _userManager.UpdateAsync(user);

                var changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

                _logger.LogInformation("User requested email change");

                var callbackUrl = Url.Page(
                    "/Account/ConfirmChangedEmail",
                    pageHandler: null,
                    values: new { userID = user.Id, changeEmailToken, newEmail },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(newEmail, "Confirm your HomeSchoolDayBook acount new email address.",
                    $"Please confirm the new email you provided to HomeSchoolDayBook by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                ConfirmMessage = "An email has been sent to the new address you provided." +
                    "Please click on the link in that email to verify your new address." +
                    "Once the new address has been verified, you may login with that address.";
            }           

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId, code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}