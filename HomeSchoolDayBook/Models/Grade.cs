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

        //CLCTODO: these data annotations would normally catch all problems with bad user entry, but 
        //right now the Helper method GetGradesFromFormData returns a list of grades from the user
        //entry, and if the user entered something that doesn't pass float.tryparse, the entry just
        //isn't added to the list that is returned, so no error message happens, the entry just gets
        //saved without that grade being saved to the database
        [RegularExpression(@"^-?[0-9]*(\.[0-9]{1,2})?$")]
        public float PointsEarned { get; set; }

        //CLCTODO: test this regular expression to make sure it is really doing what I think it is doing
        [RegularExpression(@"(?=.*[1-9])[0-9]*(\.[0-9]{1,2})?$")]
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
