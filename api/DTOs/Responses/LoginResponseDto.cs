
using api.Utilities;

namespace api.DTOs.Responses
{
    /// <summary>
    /// The LoginResponseDto class represents the response data for a login operation.
    /// </summary>
    /// 
    /// <remarks>
    /// The LoginResponseDto class contains properties that represent the user's information after a successful login.
    /// The properties include Id, CreatedBy, CreatedAt, UpdatedBy, UpdatedAt, Email, FirstName, LastName, Role, IsVerified, IsApproved, Rating, AccessToken, and RefreshToken.
    /// </remarks>
    public class LoginResponseDto
    {
        public string Id { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public AppUserRole? Role { get; set; }
        public bool IsVerified { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public double Rating { get; set; } = 0.0;
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
}