using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeSchoolDayBook.Helpers;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Models
{
    public class BasePageModel : PageModel
    {
        
        [TempData]
        public string DangerMessage { get; set; }      

        [TempData]
        public string WarningMessage { get; set; }

        [TempData]
        public string InfoMessage { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }
    }

   
}
