﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HomeSchoolDayBook.Models
{
    public class Subject
    {
        public int ID { get; set; }

        public string UserID { get; set; }

        [Display(Name = "Subject Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Active")]
        [DefaultValue(true)]
        [Required]
        public bool IsActive { get; set; }

        public ICollection<SubjectAssignment> SubjectAssignments { get; set; }
        public ICollection<Grade> Grades { get; set; }
    }
}
