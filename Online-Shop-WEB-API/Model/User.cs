﻿using System.ComponentModel.DataAnnotations;

namespace Car_WEB_API.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime ResetPasswordExpiry { get; set; }
        public List<UserOrder> UserOrders { get; set; }
    }
}
