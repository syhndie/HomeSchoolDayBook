using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Areas.Identity.Pages.Account.Manage
{
    public class SetPasswordModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;
        private readonly SignInManager<HomeSchoolDayBookUser> _signInManager;

        public SetPasswordModel(
            UserManager<HomeSchoolDayBookUser> userManager,
            SignInManager<HomeSchoolDayBookUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        
        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                DangerMessage = "Unable to load user";
                return RedirectToPage("./ChangePassword");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToPage("./ChangePassword");
            }

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
                DangerMessage = "Unable to load user.";
                return RedirectToPage("./ChangePassword");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                DangerMessage = "An error occurred when setting your password.";
                foreach (var error in addPasswordResult.Errors)
                {
                    DangerMessage = DangerMessage + error.Description;
                }
                return RedirectToPage("./ChangePassword");
            }

            await _signInManager.RefreshSignInAsync(user);
            SuccessMessage = "Your password has been set.";

            return RedirectToPage();
        }
    }
}
