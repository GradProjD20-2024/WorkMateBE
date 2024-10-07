namespace WorkMateBE.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Status { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
