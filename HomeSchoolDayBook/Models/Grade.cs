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
        
        public decimal PointsEarned { get; set; }

        [Range(double.Epsilon, double.MaxValue)]
        public decimal PointsAvailable { get; set; }

        public Entry Entry { get; set; }
        public Student Student { get; set; }
        public Subject Subject { get; set; }
    }
}
