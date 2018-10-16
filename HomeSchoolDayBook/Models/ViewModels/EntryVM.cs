using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class EntryVM
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }

        [Display(Name = "Time Spent")]
        [DataType(DataType.Text)]
        public int? EnteredHours { get; set; }

        [DataType(DataType.Text)]
        public int? EnteredMinutes { get; set; }

        public int? EnteredTotalMinutes
        {
            get
            {
                if (EnteredHours == null && EnteredMinutes == null) return null;
                else if (EnteredHours == null && EnteredMinutes != null) return EnteredMinutes;
                else if (EnteredHours != null && EnteredMinutes == null) return EnteredHours * 60;
                else return (EnteredHours * 60) + EnteredMinutes;
            }
        }

    }
}
