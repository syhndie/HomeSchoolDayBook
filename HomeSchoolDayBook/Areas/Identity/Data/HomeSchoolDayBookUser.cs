using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the HomeSchoolDayBookUser class
    public class HomeSchoolDayBookUser : IdentityUser
    {
        

        //this is not null when a user has requested to change email, but has not yet confirmed the new email address
        [Display(Name ="Pending Email")]
        public string PendingEmail { get; set; }

        [DataType(DataType.Date)]
        [Display(Name ="Account Created")]
        public DateTime AccountCreatedTimeStamp { get; set; }

        [Display(Name ="Confirmation Emails Sent")]
        public int EmailConfirmsCount { get; set; }

        [Display(Name ="Password Reset Emails Sent")]
        public int ForgotPasswordEmailsCount { get; set; }
    }
}
