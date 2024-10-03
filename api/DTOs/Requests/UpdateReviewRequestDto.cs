using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Annotations.Validation;

namespace api.DTOs.Requests
{
    public class UpdateReviewRequestDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Message { get; set; } = null!;
        [Required]
        [Range(0, 5)]
        public double Rating { get; set; } = 0;
    }
}