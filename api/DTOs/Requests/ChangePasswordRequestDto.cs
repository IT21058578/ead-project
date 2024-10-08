

using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The ChangePasswordRequestDto class represents the data required for changing the user's password.
    /// </summary>
    /// 
    /// <remarks>
    /// The ChangePasswordRequestDto class is used to provide the necessary data for changing the user's password.
    /// It contains the NewPassword property which represents the new password to be set.
    /// It contains the OldPassword property which represents the old password for verification.
    /// It contains the Email property which represents the email of the user.
    /// </remarks>
    public class ChangePasswordRequestDto
    {
        [Required]
        [MinLength(1)]
        public string NewPassword { get; set; } = "";
        [Required]
        [MinLength(1)]
        public string OldPassword { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}