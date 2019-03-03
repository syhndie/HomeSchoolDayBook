using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Authorization;

namespace HomeSchoolDayBook.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class AccessDeniedModel : BasePageModel
    {
        public void OnGet()
        {

        }
    }
}

