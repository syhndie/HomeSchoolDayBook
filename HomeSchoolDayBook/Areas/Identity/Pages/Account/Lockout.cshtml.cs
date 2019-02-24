using Microsoft.AspNetCore.Authorization;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LockoutModel : BasePageModel
    {
        public void OnGet()
        {

        }
    }
}
