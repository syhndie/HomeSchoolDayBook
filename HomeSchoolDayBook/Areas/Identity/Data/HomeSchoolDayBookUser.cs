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
        ///<summary>this is not null when a user has requested to change email, but has not yet confirmed the new email address</summary>
        [Display(Name ="Pending Email")]
        public string PendingEmail { get; set; }

        /// <summary>datetime account was created</summary>
        [DataType(DataType.Date)]
        [Display(Name ="Account Created")]
        public DateTime AccountCreatedTimeStamp { get; set; }

        /// <summary>number of emails sent to confirm email address, is reset to zero on confirmation</summary>
        [Display(Name ="Confirmation Emails Sent")]
        [Range(0, 20)]
        public int EmailConfirmsCount { get; set; }

        /// <summary>number of emails sent to confirm email address when user is changing email address. it is reset to zero on confirmation of new address</summary>
        [Display(Name ="New Email Address Confirmations Sent")]
        [Range(0, 20)]
        public int NewEmailConfirmsCount { get; set; }

        /// <summary>number of emails sent to change password, is reset to zero on password change</summary>
        [Display(Name ="Password Reset Emails Sent")]
        [Range(0, 20)]
        public int ForgotPasswordEmailsCount { get; set; }
    }
}
