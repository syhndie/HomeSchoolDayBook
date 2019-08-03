using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class EditUser
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        /// <summary>datetime account was created</summary>
        [DataType(DataType.DateTime)]
        public DateTime AccountCreatedTimeStamp { get; set; }

        /// <summary>number of emails sent to confirm email address, is reset to zero on confirmation</summary>
        [Range(0, 20)]
        public int EmailConfirmsCount { get; set; }

        /// <summary>number of emails sent to change password, is reset to zero on password change</summary>
        [Range(0,20)]
        public int ForgotPasswordEmailCount { get; set; }

        ///<summary>this is not null when a user has requested to change email, but has not yet confirmed the new email address</summary>
        [DataType(DataType.EmailAddress)]
        public string PendingEmail { get; set; }

        public EditUser()
        {

        }
        public EditUser(string email, DateTime accountCreated, bool emailConfirmed, int emailConfirmsCount, int forgotPasswordEmailsCount, string pendingEmail)
        {
            Email = email;
            AccountCreatedTimeStamp = accountCreated;
            EmailConfirmed = emailConfirmed;
            EmailConfirmsCount = emailConfirmsCount;
            ForgotPasswordEmailCount = forgotPasswordEmailsCount;
            PendingEmail = pendingEmail;
        }
    }
}
