namespace DynamicTimeTableGenerator.Models
{
    public class TimeTableModel
    {
        public List<List<string>> Schedule { get; set; }
        public int WorkingDays { get; set; }
        public int SubjectsPerDay { get; set;}
    }
}
