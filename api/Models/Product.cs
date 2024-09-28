using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Product : IMongoModel
    {
        public string? Id { get; set; }
        public string VendorId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Price { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public int CountInStock { get; set; } = 0;
    }
}