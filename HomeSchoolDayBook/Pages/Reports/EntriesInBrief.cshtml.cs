﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public EntriesReportVM EntriesReportVM {get; set;}

        public EntriesInBriefModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(string start, string end, string studentIDs)
        {
            EntriesReportVM = new EntriesReportVM(start, end, studentIDs, _context);
        }
    }
}