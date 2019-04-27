﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class SubjectGrade
    {
        public Subject Subject { get; set; }

        [Display(Name ="Points Earned")]
        public decimal PointsEarned { get; set; }

        [Display(Name ="Points Available")]
        public decimal PointsAvailable { get; set; }

        [Display(Name = "Percent Earned")]
        public decimal PercentEarned
        {
            get
            {
                return 100 * (PointsEarned / PointsAvailable);
            }
        }

        public SubjectGrade(Subject subject, decimal pointsEarned, decimal pointsAvailable)
        {
            Subject = subject;
            PointsEarned = pointsEarned;
            PointsAvailable = pointsAvailable;
        }
    }
}
