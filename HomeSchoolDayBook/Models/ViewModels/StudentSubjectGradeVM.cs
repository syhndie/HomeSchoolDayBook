using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class StudentSubjectGradeVM
    {
        [Display(Name = "Student")]
        public string StudentName { get; set; }

        [Display(Name = "Subject")]
        public string SubjectName { get; set; }

        [Display(Name = "Points Earned")]
        public decimal PointsEarned { get; set; }

        [Display(Name = "Points Available")]
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

        public StudentSubjectGradeVM(string studentName, string subjectName, decimal pointsEarned, decimal pointsAvailable)
        {
            StudentName = studentName;
            SubjectName = subjectName;
            PointsEarned = pointsEarned;
            PointsAvailable = pointsAvailable;
        }
    }
}
