using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSchoolDayBook.Models
{
    public class Enrollment
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public int EntryID { get; set; }

        public Student Student { get; set; }
        public Entry Entry { get; set; }
    }
}
