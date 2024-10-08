
using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The UpdateUserRequestDto class represents the data transfer object for updating user information.
    /// </summary>
    /// 
    /// <remarks>
    /// The UpdateUserRequestDto class is used to transfer data for updating user information.
    /// It contains properties for the first name and last name of the user.
    /// </remarks>
    public class UpdateUserRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = "";
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = "";
    }
}