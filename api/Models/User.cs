using api.Utilities;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
    /// <summary>
    /// The User class represents a user in the API.
    /// </summary>
    /// 
    /// <remarks>
    /// The User class inherits from the BaseModel class and contains properties specific to a user.
    /// These properties include the Email, FirstName, LastName, Role, IsVerified, IsApproved, and Rating properties.
    /// </remarks>
    [Collection("users")]
    public class User : BaseModel
    {
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public AppUserRole Role { get; set; } = AppUserRole.Customer;
        public bool IsVerified { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public double Rating { get; set; } = 0.0;
    }
}