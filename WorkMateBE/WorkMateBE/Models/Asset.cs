namespace WorkMateBE.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status {  get; set; }
        public int? EmployeeId { get; set; } //Assign To Employee ID
        public Employee Employee { get; set; }

    }
}
