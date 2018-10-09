using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSchoolDayBook.Models
{
    public class Subject
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<SubjectAssignment> SubjectAssignments { get; set; }
    }
}
