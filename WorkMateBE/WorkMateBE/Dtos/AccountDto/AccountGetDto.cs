﻿using WorkMateBE.Enums;

namespace WorkMateBE.Dtos.AccountDto
{
    public class AccountGetDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public string? AvatarUrl { get; set; }
        public string FaceUrl { get; set; }
        public int Status { get; set; }
        public int EmployeeId { get; set; }
    }
}
