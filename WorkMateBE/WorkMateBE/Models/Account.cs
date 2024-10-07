namespace WorkMateBE.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string role { get; set; }
        public string AvatarUrl { get; set; }
        public string FaceUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Status { get; set; }
        public int EmployeeId {  get; set; }
        public Employee Employee { get; set; }
    }
}
