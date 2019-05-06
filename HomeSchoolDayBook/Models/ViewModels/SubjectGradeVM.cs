using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class SubjectGradeVM
    {
        public Subject Subject { get; set; }

        [Display(Name ="Points Earned")]
        public decimal PointsEarned { get; set; }

        [Display(Name ="Points Available")]
        [Range(double.Epsilon, double.MaxValue)]
        public decimal PointsAvailable { get; set; }

        [Display(Name = "Percent Earned")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal PercentEarned
        {
            get
            {
                return PointsEarned / PointsAvailable;
            }
        }

        public SubjectGradeVM(Subject subject, decimal pointsEarned, decimal pointsAvailable)
        {
            Subject = subject;
            PointsEarned = pointsEarned;
            PointsAvailable = pointsAvailable;
        }
    }
}
