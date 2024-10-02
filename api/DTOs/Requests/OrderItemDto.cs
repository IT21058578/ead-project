using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Annotations.Validation;

namespace api.DTOs.Requests
{
    public class OrderItemDto
    {
        [Required]
        [ValidObjectId]
        public string ProductId { get; set; } = string.Empty;
        [Required]
        [ValidObjectId]
        public string VendorId { get; set; } = string.Empty;
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 0;
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        [Range(0.01, double.MaxValue)]
        public double Price { get; set; }
    }
}