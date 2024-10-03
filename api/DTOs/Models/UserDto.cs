using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Utilities;

namespace api.DTOs.Models
{
    public class UserDto
    {
        public string Id { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public AppUserRole Role { get; set; } = AppUserRole.Customer;
        public bool IsVerified { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public double Rating { get; set; } = 0.0;
    }
}