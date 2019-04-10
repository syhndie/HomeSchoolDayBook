using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Models
{
    public class Grade
    {
        public int ID { get; set; }
        public int EntryID { get; set; }
        public int StudentID { get; set; }
        public int SubjectID { get; set; }

        public float PointsEarned { get; set; }

        [Range(0.1, float.PositiveInfinity)]
        public float PointsAvailable { get; set; }

        public Entry Entry { get; set; }
        public Student Student { get; set; }
        public Subject Subject { get; set; }

        public float Percent
        {
            get
            { 
                return PointsEarned / PointsAvailable;
            }
        }
    }
}
