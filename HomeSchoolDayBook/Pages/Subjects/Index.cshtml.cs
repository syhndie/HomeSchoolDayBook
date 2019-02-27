using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeSchoolDayBook.Models;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using HomeSchoolDayBook.Data;

namespace HomeSchoolDayBook.Pages.Subjects
{
    public class IndexModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public IList<Subject> Subjects { get; set; }

        public IndexModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }              

        public async void OnGetAsync()
        {
            string userId = _userManager.GetUserId(User);

            Subjects = await _context.Subjects
                .Where(su => su.UserID == userId)
                .ToListAsync();
        }
    }
}
