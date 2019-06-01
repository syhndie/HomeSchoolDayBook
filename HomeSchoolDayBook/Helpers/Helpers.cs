using System.Collections.Generic;
using System.Linq;
using HomeSchoolDayBook.Models;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace HomeSchoolDayBook.Helpers
{
    public static class Helpers
    {
        public static List<Grade> GetGradesFromFormData(IFormCollection formData, Entry entry, string userID, out bool allGradesValid)
        {
            allGradesValid = true;

            List<Grade> grades = new List<Grade>();

            string[] earnedFieldNames = formData.Keys.Where(k => k.StartsWith("earned")).ToArray();

            foreach (string efn in earnedFieldNames)
            {
                bool earnedIsEmpty = formData[efn] == "" ? true : false;

                string[] parts = efn.Split('-');
                int studentId = int.Parse(parts[2]);
                int subjectId = int.Parse(parts[4]);

                string afn = string.Join('-', new string[] { "available", parts[1], parts[2], parts[3], parts[4] });

                bool availableIsEmpty = formData[afn] == "" ? true : false;

                //returns out-variable allGradesValid as false if one grade field null but not the other
                if ((earnedIsEmpty && !availableIsEmpty) || (!earnedIsEmpty && availableIsEmpty)) allGradesValid = false;

                if (!decimal.TryParse(formData[efn], out decimal earnedValue))
                {
                    if (!earnedIsEmpty) allGradesValid = false;
                    continue;
                }

                if (!decimal.TryParse(formData[afn], out decimal availableValue))
                {
                    if (!availableIsEmpty) allGradesValid = false;
                    continue;
                }

                var newGrade = new Grade
                {
                    StudentID = studentId,
                    SubjectID = subjectId,
                    UserID = userID,
                    PointsEarned = earnedValue,
                    PointsAvailable = availableValue,
                    EntryID = entry.ID
                };

                grades.Add(newGrade);
            }

            return grades;
        }

        public static string GetStringFromList(List<string> listOfStrings)
        {
            listOfStrings.Sort();

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < listOfStrings.Count; i++)
            {
                if (i == 0) stringBuilder.Append(listOfStrings[i]);

                else if (i == listOfStrings.Count - 1) stringBuilder.Append($" and {listOfStrings[i]}");

                else stringBuilder.Append($", {listOfStrings[i]}");
            }

            return stringBuilder.ToString();         
        }

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
