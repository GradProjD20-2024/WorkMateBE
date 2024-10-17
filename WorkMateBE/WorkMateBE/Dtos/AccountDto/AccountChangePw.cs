using System.ComponentModel.DataAnnotations;

namespace WorkMateBE.Dtos.AccountDto
{
    public class AccountChangePw
    {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string ConfirmPassword { get; set; }
    }
}
