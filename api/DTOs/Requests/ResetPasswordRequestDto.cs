using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Requests
{
    public class ResetPasswordRequestDto
    {
        [Required]
        [MinLength(1)]
        public string NewPassword { get; set; } = "";
        [Required]
        [MinLength(1)]
        public string Code { get; set; } = "";
    }
}