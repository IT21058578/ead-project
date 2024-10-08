

using api.Utilities;

namespace api.DTOs.Models
{
    /// <summary>
    /// The UserDto class represents the data transfer object for a user.
    /// </summary>
    /// 
    /// <remarks>
    /// The UserDto class is used to transfer data for a user.
    /// It contains properties for the user ID, created by, created at,
    /// updated by, updated at, email, first name, last name, role,
    /// verification status, approval status, and rating of the user.
    /// </remarks>
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