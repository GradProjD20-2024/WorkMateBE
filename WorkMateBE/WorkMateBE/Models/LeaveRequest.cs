namespace WorkMateBE.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Status {  get; set; }//0: pending, 1: approval, 2: reject
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
