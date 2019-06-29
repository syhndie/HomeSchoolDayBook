using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Data;
using Microsoft.AspNetCore.Authorization;
using static HomeSchoolDayBook.Helpers.Constants;

namespace HomeSchoolDayBook.Pages.Admin
{
    [Authorize(Roles = AdminRoleName)]
    public class EditModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public HomeSchoolDayBookUser UserToEdit { get; set; }

        public EditModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public void OnGet(string userToEditId)
        {
            UserToEdit = _userManager.Users.Single(u => u.Id == userToEditId);
        }
    }
}