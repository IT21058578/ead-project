
using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The RefreshLoginRequestDto class represents the data transfer object for refreshing login.
    /// </summary>
    /// 
    /// <remarks>
    /// The RefreshLoginRequestDto class is used to transfer the refresh token for refreshing the login session.
    /// The RefreshToken property is required and should contain the refresh token string.
    /// </remarks>
    public class RefreshLoginRequestDto
    {
        [Required]
        // TODO: JWT Token data validation
        public string RefreshToken { get; set; } = "";
    }
}