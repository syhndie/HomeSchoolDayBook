﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HomeSchoolDayBook.Pages.Grades
{
    public class GradesForEntriesModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        [Display(Name = "From")]
        public DateTime FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        [Display(Name = "To")]
        public DateTime ToDate { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Subject")]
        public int SubjectID { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Student")]
        public int StudentID { get; set; }

        public string StudentName { get; set; }

        public string SubjectName { get; set; }

        public List<SelectListItem> SubjectOptions { get; set; }

        public List<SelectListItem> StudentOptions { get; set; }

        public List<Grade> Grades {get; set;}

   
        public decimal OverallPointsEarned { get; set; }

        public decimal OverallPointsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal OverallPercentEarned { get; set; }

        public GradesForEntriesModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(DateTime? fromDate, DateTime? toDate, int subjectID, int studentID)
        {
            string userId = _userManager.GetUserId(User);

            if (fromDate == null) FromDate = DateTime.Today.AddMonths(-1);

            if (toDate == null) ToDate = DateTime.Today;

            List<Subject> subjectList = await _context
                .Subjects
                .Where(su => su.UserID == userId)
                .OrderByDescending(su => su.IsActive)
                .ThenBy(su => su.Name)
                .ToListAsync();

            SubjectOptions = subjectList
                .Select(su => new SelectListItem { Value = su.ID.ToString(), Text = su.Name })
                .ToList();


            SubjectName = subjectList.FirstOrDefault(su => su.ID == subjectID).Name;

            List<Student> studentList = await _context
                .Students
                .Where(st => st.UserID == userId)
                .OrderByDescending(st => st.IsActive)
                .ThenBy(st => st.Name)
                .ToListAsync();

            StudentOptions = studentList
                .Select(st => new SelectListItem { Value = st.ID.ToString(), Text = st.Name })
                .ToList();

            StudentName = studentList.FirstOrDefault(st => st.ID == studentID).Name;

            DateTime startDate = FromDate <= ToDate ? (DateTime)FromDate : (DateTime)ToDate;
            DateTime endDate = startDate == (DateTime)FromDate ? (DateTime)ToDate : (DateTime)FromDate;

            Grades = await _context
                .Grades
                .Where(gr => gr.UserID == userId)
                .Where(gr => gr.SubjectID == subjectID)
                .Where(gr => gr.StudentID == studentID)
                .Where(gr => gr.Entry.Date >= startDate)
                .Where(gr => gr.Entry.Date <= endDate)
                .Include(gr => gr.Entry)
                .OrderBy(gr => gr.Entry.Date)
                    .ThenBy(gr => gr.Entry.Title)
                .ToListAsync();

            if (Grades.Count > 0)
            {
                OverallPointsEarned = Grades.Sum(gr => gr.PointsEarned);
                OverallPointsAvailable = Grades.Sum(gr => gr.PointsAvailable);
                OverallPercentEarned = (OverallPointsEarned / OverallPointsAvailable);
            }

            return Page();
        }
    }
}