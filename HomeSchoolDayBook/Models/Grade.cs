using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using static HomeSchoolDayBook.Helpers.Constants;

namespace HomeSchoolDayBook.Models
{
    public class Grade
    {
        public int ID { get; set; }
        public int EntryID { get; set; }
        public int StudentID { get; set; }
        public int SubjectID { get; set; }

        [RegularExpression(PointsEarnedPattern)]
        public decimal PointsEarned { get; set; }

        [Range(0.01, double.MaxValue)]
        [RegularExpression(PointsAvailablePattern)]
        [Required]
        public decimal PointsAvailable { get; set; }

        public Entry Entry { get; set; }
        public Student Student { get; set; }
        public Subject Subject { get; set; }
    }
}
