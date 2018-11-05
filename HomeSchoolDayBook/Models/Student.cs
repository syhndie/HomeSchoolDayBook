using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Models
{
    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Display(Name ="Active?")]
        public bool IsActive { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
