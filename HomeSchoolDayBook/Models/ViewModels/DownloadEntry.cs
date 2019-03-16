using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeSchoolDayBook.Models;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class DownloadEntry
    {
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public int? MinutesSpent { get; set; }

        public string Students { get; set; }

        public string Subjects { get; set; }

        public DownloadEntry(Entry entry, string students, string subjects)
        {
            Title = entry.Title;
            Date = entry.Date;
            Description = entry.Description;
            MinutesSpent = entry.MinutesSpent;
            Students = students;
            Subjects = subjects;
        }
    }
}
