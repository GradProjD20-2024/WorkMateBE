using WorkMateBE.Enums;

namespace WorkMateBE.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? AvatarUrl { get; set; }
        public string? FaceUrl { get; set; }
        public string? FaceId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Status { get; set; }
        public int EmployeeId {  get; set; }
        public Employee Employee { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
