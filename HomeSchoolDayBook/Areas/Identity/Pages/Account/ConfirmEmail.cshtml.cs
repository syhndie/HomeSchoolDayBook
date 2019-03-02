using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using HomeSchoolDayBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        public ConfirmEmailModel(UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                DangerMessage = $"Unable to load user with ID '{userId}'.";
                return RedirectToPage("./Index");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                DangerMessage = $"Error confirming email for user with ID '{userId}'.";
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
