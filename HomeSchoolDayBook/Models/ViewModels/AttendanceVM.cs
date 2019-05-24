
namespace HomeSchoolDayBook.Models.ViewModels
{
    public class AttendanceVM
    {
        public string StudentName { get; set; }

        public int DaysAttended { get; set; }

        public AttendanceVM (string studentName, int daysAttended)
        {
            StudentName = studentName;
            DaysAttended = daysAttended;
        }
    }
}
