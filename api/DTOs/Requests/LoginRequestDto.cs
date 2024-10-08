
using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The LoginRequestDto class represents the data transfer object for login requests.
    /// </summary>
    /// 
    /// <remarks>
    /// The LoginRequestDto class is used to transfer login request data between the client and the server.
    /// /// It contains properties for the email and password fields, which are required for authentication.
    /// </remarks>
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = "";
    }
}