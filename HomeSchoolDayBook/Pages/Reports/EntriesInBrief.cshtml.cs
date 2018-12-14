using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Data;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Models.ViewModels;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class EntriesInBriefModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
       
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string StudentNames { get; set; }

        public List<Entry> Entries { get; set; }

        public EntriesInBriefModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(string start, string end, string studentIDs)
        {
            StartDate = Convert.ToDateTime(start);

            EndDate = Convert.ToDateTime(end);

            List<int> studentIntIDs = studentIDs is null || studentIDs == "" 
                ? new List<int>() :
                studentIDs.Split(',')
                .Select(Int32.Parse)
                .ToList();

            List<string> studentNamesList = _context.Students
                .Where(s => studentIntIDs.Contains(s.ID))
                .OrderBy(s => s.Name)
                .Select(s => s.Name)
                .ToList();

            StudentNames = String.Join(", ", studentNamesList);

            Entries = _context.Entries
                .Include(ent => ent.Enrollments)
                    .ThenInclude(enr => enr.Student)
                .Include(ent => ent.SubjectAssignments)
                    .ThenInclude(sa => sa.Subject)
                .Where(ent => StartDate <= ent.Date && ent.Date <= EndDate)
                .Where(e => e.Enrollments.Any(r => studentIntIDs.Contains(r.StudentID)))
                .ToList();         
        }
    }
}