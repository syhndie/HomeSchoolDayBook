﻿using System.Threading.Tasks;
using HomeSchoolDayBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Areas.Identity.Pages.Account.Manage
{
    public class TwoFactorAuthenticationModel : BasePageModel
    {
        //private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}";

        //private readonly UserManager<HomeSchoolDayBookUser> _userManager;
        //private readonly SignInManager<HomeSchoolDayBookUser> _signInManager;
        //private readonly ILogger<TwoFactorAuthenticationModel> _logger;

        //public TwoFactorAuthenticationModel(
        //    UserManager<HomeSchoolDayBookUser> userManager,
        //    SignInManager<HomeSchoolDayBookUser> signInManager,
        //    ILogger<TwoFactorAuthenticationModel> logger)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _logger = logger;
        //}

        //public bool HasAuthenticator { get; set; }

        //public int RecoveryCodesLeft { get; set; }

        //[BindProperty]
        //public bool Is2faEnabled { get; set; }

        //public bool IsMachineRemembered { get; set; }

        //[TempData]
        //public string StatusMessage { get; set; }

        public void OnGet()
        {

        }

        //public async Task<IActionResult> OnPost()
        //{
            //var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            //}

            //await _signInManager.ForgetTwoFactorClientAsync();
            //StatusMessage = "The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.";
        //    return RedirectToPage();
        //}
    }
}