using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Requests
{
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