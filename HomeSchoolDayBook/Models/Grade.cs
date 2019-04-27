using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HomeSchoolDayBook.Models
{
    public class Grade
    {
        [JsonIgnore]
        public int ID { get; set; }

        [JsonIgnore]
        public int EntryID { get; set; }

        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        
        [Display(Name = "Points Earned")]
        public decimal PointsEarned { get; set; }

        [Display(Name = "Points Available")]
        [Range(double.Epsilon, double.MaxValue)]
        public decimal PointsAvailable { get; set; }

        [Display(Name = "Percent")]
        public decimal PercentEarned
        {
            get
            {
                return 100*( PointsEarned / PointsAvailable);
            }
        }

        public Entry Entry { get; set; }
        public Student Student { get; set; }
        public Subject Subject { get; set; }
    }
}
