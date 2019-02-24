using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Pages
{
    [AllowAnonymous]
    public class PrivacyModel : BasePageModel
    {
        public void OnGet()
        {
        }
    }
}