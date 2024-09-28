using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Requests
{
    public class CreateProductRequestDto
    {
        [Required]
        public string VendorId { get; set; } = string.Empty;
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Description { get; set; } = null!;
        [Required]
        public string Category { get; set; } = null!;
        [Required]
        [Range(0.01, double.MaxValue)]
        public double Price { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        [Required]
        [Range(0, int.MaxValue)]
        public int CountInStock { get; set; } = 0;
        [Required]
        [Range(0, int.MaxValue)]
        public int LowStockThreshold { get; set; } = 10;
        [Url]
        [Required]
        public string ImageUrl { get; set; } = null!;
    }
}