﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class StudentOrSubjectEditOrCreate
    {
        [Required]
        public string Name { get; set; }

        [Display(Name = "Active")]
        [DefaultValue(true)]
        [Required]
        public bool IsActive { get; set; }
    }
}
