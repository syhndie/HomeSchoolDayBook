using System;
using System.Collections.Generic;
using System.Linq;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Areas.Identity.Data;
using static HomeSchoolDayBook.Helpers.Constants;
using Microsoft.AspNetCore.Authorization;

namespace HomeSchoolDayBook.Pages.Admin
{
    [Authorize(Roles = AdminRoleName)]
    public class IndexModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public List<HomeSchoolDayBookUser> AllUsers { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            AllUsers = _context.Users
                .OrderBy(u => u.UserName)
                .ToList();
        }
    }
}