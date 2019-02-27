using Microsoft.AspNetCore.Authorization;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Pages
{
    [AllowAnonymous]
    public class IndexModel : BasePageModel
    {
        public void OnGet()
        {            

        }
    }
}