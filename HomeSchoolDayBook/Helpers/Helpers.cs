using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeSchoolDayBook.Models;
using System.Text;

namespace HomeSchoolDayBook.Helpers
{
    public static class Helpers
    {
        public static string GetTimeSpentDisplay(int? totalMinutes)
        {
            if (totalMinutes == null) return "Time was not recorded.";

            if (totalMinutes == 0) return "0 minutes";

            int? hours = totalMinutes / 60;
            int? minutes = totalMinutes % 60;

            string hoursUnits = hours == 1 ? "hour" : "hours";
            string minutesUnits = minutes == 1 ? "minute" : "minutes";

            string hoursString = hours == 0  ? "" : $"{hours} {hoursUnits}";
            string minutesString = minutes == 0 ? "" : $"{minutes} {minutesUnits}";

            return (hoursString == "" || minutesString == "") ? $"{hoursString}{minutesString}" : $"{hoursString}, {minutesString}";
        }

        public static string GetStudentNames(Entry entry)
        {
            List<Enrollment> enrollments = entry.Enrollments.OrderBy(enr => enr.Student.Name).ToList();

            StringBuilder studentNames = new StringBuilder();

            for (int i = 0; i < entry.Enrollments.Count(); i++)
            {
                if (i == 0)
                {
                    studentNames.Append(enrollments[i].Student.Name);
                }
                else if (i == entry.Enrollments.Count() - 1)
                {
                    studentNames.Append($" and {enrollments[i].Student.Name}");
                }
                else
                {
                    studentNames.Append($", {enrollments[i].Student.Name}");
                }
            }
            return studentNames.ToString();
        }

        public static string GetSubjectNames(Entry entry)
        {
            List<SubjectAssignment> subjectAssignments = entry.SubjectAssignments.OrderBy(sa => sa.Subject.Name).ToList();

            StringBuilder subjectNames = new StringBuilder();

            for (int i = 0; i < entry.SubjectAssignments.Count(); i++)
            {
                if (i == 0)
                {
                    subjectNames.Append(subjectAssignments[i].Subject.Name);
                }
                else if (i == entry.SubjectAssignments.Count() - 1)
                {
                    subjectNames.Append($" and {subjectAssignments[i].Subject.Name}");
                }
                else
                {
                    subjectNames.Append($", {subjectAssignments[i].Subject.Name}");
                }
            }

            return subjectNames.ToString();
        }

    }
}
