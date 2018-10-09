using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSchoolDayBook.Models
{
    public class SubjectAssignment
    {
        public int ID { get; set; }
        public int SubjectID { get; set; }
        public int EntryID { get; set; }

        public Subject Subject { get; set; }
        public Entry Entry { get; set; }
    }
}
