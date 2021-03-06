﻿using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : BasePageModel
    {
        private readonly HomeSchoolDayBook.Data.ApplicationDbContext _context;
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;
        private readonly SignInManager<HomeSchoolDayBookUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<IndexModel> _logger;

        private const int maxAllowedEmails = 5;

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
                DangerMessage = "Unable to load user";
                return RedirectToPage();
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
                DangerMessage = "Unable to load user";
                return RedirectToPage();
            }

            var newEmail = Input.Email;
            var oldEmail = await _userManager.GetEmailAsync(user);

            if (newEmail != oldEmail)
            {
                if (_context.Users.Any(u => u.Email == newEmail))
                {
                    DangerMessage = "An error occurred when changing your email address. The new email address may already exist in our system.";
                    return RedirectToPage();
                }

                if (user.NewEmailConfirmsCount <= maxAllowedEmails)
                {
                    user.PendingEmail = newEmail;
                    user.NewEmailConfirmsCount++;
                    await _userManager.UpdateAsync(user);

                    var changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

                    _logger.LogInformation("User requested email change");

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmChangedEmail",
                        pageHandler: null,
                        values: new { userID = user.Id, changeEmailToken, newEmail },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(newEmail, "Confirm your HomeSchoolDayBook account new email address.",
                        $"Please confirm the new email you provided to HomeSchoolDayBook by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    InfoMessage = "An email has been sent to the new address you provided." +
                        "Please click on the link in that email to verify your new address." +
                        "Once the new address has been verified, you may login with that address.";
                }
                else DangerMessage = $"A maximum of {maxAllowedEmails} new email confirmation messages has been sent."+
                        "If you are having trouble confirming your new email address, please contact cindy@homeschooldaybook.com.";
            }           

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }
    }
}