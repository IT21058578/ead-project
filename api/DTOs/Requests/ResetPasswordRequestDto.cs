
using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The ResetPasswordRequestDto class represents the data transfer object for resetting a user's password.
    /// </summary>
    /// 
    /// <remarks>
    /// The ResetPasswordRequestDto class is used to transfer the necessary information for resetting a user's password.
    /// It contains the following properties:
    /// - NewPassword: The new password to be set for the user. (Required, Minimum length: 1)
    /// - Code: The verification code for resetting the password. (Required, Minimum length: 1)
    /// - Email: The email address of the user. (Required, Email format)
    /// </remarks>
    public class ResetPasswordRequestDto
    {
        [Required]
        [MinLength(1)]
        public string NewPassword { get; set; } = "";
        [Required]
        [MinLength(1)]
        public string Code { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}