
using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The CreateUserRequestDto class represents the data transfer object for creating a user.
    /// </summary>
    /// 
    /// <remarks>
    /// The CreateUserRequestDto class is used to transfer data for creating a user.
    /// It contains properties for the email, password, first name, and last name of the user.
    /// The email property is required and must be a valid email address.
    /// The password property is required and must have a minimum length of 8 characters.
    /// The first name and last name properties are required and must have a maximum length of 50 characters.
    /// </remarks>
    public class CreateUserRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = "";
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = "";
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = "";
    }
}