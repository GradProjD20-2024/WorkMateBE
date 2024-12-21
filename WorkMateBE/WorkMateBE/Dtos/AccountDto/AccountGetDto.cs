using WorkMateBE.Enums;

namespace WorkMateBE.Dtos.AccountDto
{
    public class AccountGetDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string? AvatarUrl { get; set; }
        public string FaceUrl { get; set; }
        public string FaceId {  get; set; }
        public int Status { get; set; }
        public int EmployeeId { get; set; }
    }
}
