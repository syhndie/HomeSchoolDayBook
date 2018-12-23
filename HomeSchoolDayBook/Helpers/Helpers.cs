using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSchoolDayBook.Helpers
{
    public static class Helpers
    {
        public static string GetTimeSpentDisplay(int? totalMinutes)
        {
            if (totalMinutes == null) return null;

            if (totalMinutes == 0) return "0 minutes";

            int? hours = totalMinutes / 60;
            int? minutes = totalMinutes % 60;

            string hoursUnits = hours == 1 ? "hour" : "hours";
            string minutesUnits = minutes == 1 ? "minute" : "minutes";

            string hoursString = hours == 0  ? "" : $"{hours} {hoursUnits}";
            string minutesString = minutes == 0 ? "" : $"{minutes} {minutesUnits}";

            return (hoursString == "" || minutesString == "") ? $"{hoursString}{minutesString}" : $"{hoursString}, {minutesString}";
        }
    }
}
