using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Models
{
    public class ReviewDto
    {
        public string Id { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
        public string VendorId { get; set; } = "";
        public string ProductId { get; set; } = "";
        public string UserId { get; set; } = "";
        public string Message { get; set; } = "";
        public double Rating { get; set; } = 0;
    }
}