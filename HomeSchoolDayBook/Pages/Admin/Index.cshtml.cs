﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeSchoolDayBook.Models;
using HomeSchoolDayBook.Data;
using HomeSchoolDayBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using HomeSchoolDayBook.Areas.Identity.Data;
using static HomeSchoolDayBook.Helpers.Helpers;
using Microsoft.EntityFrameworkCore;
using static HomeSchoolDayBook.Helpers.Constants;
using Microsoft.AspNetCore.Authorization;

namespace HomeSchoolDayBook.Pages.Admin
{
    [Authorize(Roles = AdminRoleName)]
    public class IndexModel : BasePageModel
    {
        private readonly UserManager<HomeSchoolDayBookUser> _userManager;

        private readonly ApplicationDbContext _context;

        public List<HomeSchoolDayBookUser> AllUsers { get; set; }

        public IndexModel(ApplicationDbContext context, UserManager<HomeSchoolDayBookUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public void OnGet()
        {
            AllUsers = _userManager.Users
                .OrderBy(u => u.UserName)
                .ToList();
        }
    }
}