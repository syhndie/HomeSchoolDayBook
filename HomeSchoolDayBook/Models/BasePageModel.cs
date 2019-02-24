using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeSchoolDayBook.Models
{
    public class BasePageModel : PageModel
    {
        public class Message
        {
            public string MessageClass { get; set; }
            public string MessageText { get; set; }
        }
        [TempData]
        public List<Message> Messages { get; set; }        
    }

   
}
