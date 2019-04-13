using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HomeSchoolDayBook.Helpers
{
    public static class Constants
    {
        public static readonly string AdminRoleName = "Admin";
        public static readonly string AdminName = "admin@homeschooldaybook.com";

        //CLCTODO: double check that these regexs work
        public const string PointsEarnedPattern = @"^-?[0-9]*(\.[0-9]{1,2})?$";
        public const string PointsAvailablePattern = @"(?=.*[1-9])[0-9]*(\.[0-9]{1,2})?$";
        public const string PointsEarnedErrorMessage = "Points earned must be numbers with a maximum of two decimal places.";
        public const string PointsAvailableErrorMessage = "Points available must be positive numbers with a maximum of two decimal places.";
    }
}
