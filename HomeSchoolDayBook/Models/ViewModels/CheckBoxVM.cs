using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSchoolDayBook.Models.ViewModels
{
    public class CheckBoxVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }

        public CheckBoxVM(int id, string name, bool assigned)
        {
            ID = id;
            Name = name;
            Assigned = assigned;        
        }
    }
}
