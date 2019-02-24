using Microsoft.AspNetCore.Authorization;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Pages
{
    [AllowAnonymous]
    public class ContactModel : BasePageModel
    {
        public string ContactMessage { get; set; }

        public void OnGet()
        {
            ContactMessage = "Your contact page.";
        }
    }
}
