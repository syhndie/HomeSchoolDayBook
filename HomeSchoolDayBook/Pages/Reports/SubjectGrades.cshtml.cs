using System;
using System.Collections.Generic;
using System.Linq;
using HomeSchoolDayBook.Data;
using System.ComponentModel.DataAnnotations;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeSchoolDayBook.Pages.Reports
{
    public class SubjectGradesModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public List<StudentSubjectGradeVM> StudentGradesBySubject { get; set; }      

        public SubjectGradesModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public void OnGet(string start, string end, string studentIDs)
        {
            string userId = _userManager.GetUserId(User);

            StartDate = Convert.ToDateTime(start);

            EndDate = Convert.ToDateTime(end);

            if (studentIDs == null || studentIDs == "") StudentGradesBySubject = new List<StudentSubjectGradeVM>();
            else
            {
                List<int> studentIDList = studentIDs.Split(',')
                    .Select(int.Parse)
                    .ToList();
                
                StudentGradesBySubject = _context.Grades
                    .Where(gr => gr.UserID == userId)
                    .Include(gr => gr.Student)
                    .Include(gr => gr.Subject)
                    .Where(gr => StartDate <= gr.Entry.Date)
                    .Where(gr => gr.Entry.Date <= EndDate)
                    .Where(gr => studentIDList.Contains(gr.StudentID)).ToList()
                    .GroupBy(gr => new { gr.Student, gr.Subject }).ToList()
                    .Select(x => new StudentSubjectGradeVM(x.Key.Student.Name, x.Key.Subject.Name, x.Sum(y => y.PointsEarned), x.Sum(y => y.PointsAvailable)))                    
                    .ToList();
            }
        }
    }
}