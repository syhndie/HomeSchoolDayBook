using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HomeSchoolDayBook.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the HomeSchoolDayBookUser class
    public class HomeSchoolDayBookUser : IdentityUser
    {
        public string PendingEmail { get; set; }
        public DateTime AccountCreatedTimeStamp { get; set; }
        public int EmailConfirmsCount { get; set; }
        public int ForgotPasswordEmailsCount { get; set; }
    }
}
