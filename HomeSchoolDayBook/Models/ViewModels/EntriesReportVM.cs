using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class EntriesReportVM
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string StudentNames { get; set; }

        public List<Entry> Entries { get; set; }

        public EntriesReportVM(string start, string end, string studentIDs, ApplicationDbContext context, string userId)
        {
            StartDate = Convert.ToDateTime(start);

            EndDate = Convert.ToDateTime(end);

            List<int> studentIntIDs = studentIDs is null || studentIDs == ""
                ? new List<int>() :
                studentIDs.Split(',')
                .Select(Int32.Parse)
                .ToList();

            var studentNamesList = context.Students
                .Where(st => st.UserID == userId)
                .Where(st => studentIntIDs.Contains(st.ID))
                .OrderBy(s => s.Name)
                .Select(s => s.Name)
                .ToList();

            StudentNames = "";

            for (int i = 0; i < studentNamesList.Count(); i++)
            {
                if (i == 0) StudentNames = studentNamesList[i];

                else if (i == studentNamesList.Count() - 1) StudentNames = $"{StudentNames} and {studentNamesList[i]}";
 
                else StudentNames = $"{StudentNames}, {studentNamesList[i]}";
            }

            Entries = context.Entries
                .Where(ent => ent.UserID == userId)
                .Include(ent => ent.Enrollments)
                    .ThenInclude(enr => enr.Student)
                .Include(ent => ent.SubjectAssignments)
                    .ThenInclude(sa => sa.Subject)
                .Include(ent => ent.Grades)
                .Where(ent => StartDate <= ent.Date && ent.Date <= EndDate)
                .Where(ent => ent.Enrollments.Any(enr => studentIntIDs.Contains(enr.StudentID)))
                .OrderBy(ent => ent.Date)
                    .ThenBy(ent => ent.Title)
                .ToList();
        }

    }
}