using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSchoolDayBook.Models
{
    public class Grade
    {
        public int ID { get; set; }
        public int EntryID { get; set; }
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public int PointsEarned { get; set; }
        public int PointsAvailable { get; set; }

        public Entry Entry { get; set; }
        public Student Student { get; set; }
        public Subject Subject { get; set; }

        public decimal Percent
        {
            get
            {
                return PointsEarned / PointsAvailable;
            }
        }
    }
}
