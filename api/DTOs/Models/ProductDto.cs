using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Models
{
    public class ProductDto
    {
        public string Id { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
        public string VendorId { get; set; } = "";
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public double Price { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public int CountInStock { get; set; } = 0;
        public int LowStockThreshold { get; set; } = 10;
        public double Rating { get; set; } = 0;
        public string? ImageUrl { get; set; }
    }
}