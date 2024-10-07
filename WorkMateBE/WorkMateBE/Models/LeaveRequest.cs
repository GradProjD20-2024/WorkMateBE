namespace WorkMateBE.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Status {  get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
