using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using static HomeSchoolDayBook.Helpers.Constants;
using Microsoft.AspNetCore.Authorization;

namespace HomeSchoolDayBook.Pages.Admin
{
    [Authorize(Roles = AdminRoleName)]
    public class DeleteModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<HomeSchoolDayBookUser> _userManager;
        
        public HomeSchoolDayBookUser UserToDelete { get; set; }

        public DeleteModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userToDeleteId)
        {
            if (userToDeleteId == null)
            {
                DangerMessage = "User not found.";

                return RedirectToPage("./Index");
            }

            UserToDelete = await _userManager.Users.Where(u => u.Id == userToDeleteId).FirstOrDefaultAsync();

            if (UserToDelete == null)
            {
                DangerMessage = "User not found.";

                return RedirectToPage("./Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userToDeleteId)
        {
            if (userToDeleteId == null)
            {
                DangerMessage = "User not found.";

                return RedirectToPage("./Index");
            }

            UserToDelete = await _userManager.Users.Where(u => u.Id == userToDeleteId).FirstOrDefaultAsync();

            if (UserToDelete == null)
            {
                DangerMessage = "User not found.";

                return RedirectToPage("./Index");
            }

            if (UserToDelete.UserName == User.Identity.Name)
            {
                DangerMessage = "You cannot delete yourself from the application.";

                return RedirectToPage("./Index");
            }

            List<Entry> userEntries = await _context.Entries
                .Where(ent => ent.UserID == userToDeleteId)
                .Include(ent => ent.Enrollments)
                .Include(ent => ent.SubjectAssignments)
                .Include(ent => ent.Grades)
                .ToListAsync();

            List<Student> userStudents = await _context.Students
                .Where(st => st.UserID == userToDeleteId)
                .ToListAsync();

            List<Subject> userSubjects = await _context.Subjects
                .Where(su => su.UserID == userToDeleteId)
                .ToListAsync();

            List<Enrollment> userEnrollments = userEntries
                .SelectMany(ent => ent.Enrollments)
                .ToList();

            List<SubjectAssignment> userSubjectAssignments = userEntries
                .SelectMany(ent => ent.SubjectAssignments)
                .ToList();

            List<Grade> userGrades = userEntries
                .SelectMany(ent => ent.Grades)
                .ToList();

            using (var transaction =  await _context.Database.BeginTransactionAsync())
            {
                foreach (Entry entry in userEntries) _context.Remove(entry);

                foreach (Student student in userStudents) _context.Remove(student);

                foreach (Subject subject in userSubjects) _context.Remove(subject);

                foreach (Enrollment enrollment in userEnrollments) _context.Remove(enrollment);

                foreach (SubjectAssignment subjectAssignment in userSubjectAssignments) _context.Remove(subjectAssignment);

                foreach (Grade grade in userGrades) _context.Remove(grade);

                await _context.SaveChangesAsync();

                await _userManager.DeleteAsync(UserToDelete);

                transaction.Commit();

            }

            SuccessMessage = $"User and user data deleted.";
            return RedirectToPage("./Index");
        }        
    }
}