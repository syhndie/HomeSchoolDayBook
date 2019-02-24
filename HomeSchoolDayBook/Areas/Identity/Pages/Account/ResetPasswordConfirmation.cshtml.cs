using Microsoft.AspNetCore.Authorization;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordConfirmationModel : BasePageModel
    {
        public void OnGet()
        {

        }
    }
}
