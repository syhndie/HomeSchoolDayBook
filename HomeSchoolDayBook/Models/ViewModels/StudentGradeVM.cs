using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class StudentGradeVM
    {
        public Student Student { get; set; }

        [Display(Name = "Points Earned")]
        public decimal PointsEarned { get; set; }

        [Display(Name = "Points Available")]
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

        public StudentGradeVM(Student student, decimal pointsEarned, decimal pointsAvailable)
        {
            Student = student;
            PointsEarned = pointsEarned;
            PointsAvailable = pointsAvailable;
        }
    }
}
