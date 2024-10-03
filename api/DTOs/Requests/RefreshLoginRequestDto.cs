using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Requests
{
    public class RefreshLoginRequestDto
    {
        [Required]
        // TODO: JWT Token data validation
        public string RefreshToken { get; set; } = "";
    }
}