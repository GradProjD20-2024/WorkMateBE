using System.ComponentModel.DataAnnotations;
using WorkMateBE.Enums;

namespace WorkMateBE.Dtos.AccountDto
{
    public class AccountCreateDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public UserRole Role { get; set; }

        //[Url(ErrorMessage = "Invalid URL format for AvatarUrl")]
        public string? AvatarUrl { get; set; }

        //[Required(ErrorMessage = "FaceUrl is required")]
        //[Url(ErrorMessage = "Invalid URL format for FaceUrl")]
        public string FaceUrl { get; set; }

        [Range(0, 1, ErrorMessage = "Status must be either 0 (Inactive) or 1 (Active)")]
        public int Status { get; set; }

        [Required(ErrorMessage = "EmployeeId is required")]
        public int EmployeeId { get; set; }
    }
}
