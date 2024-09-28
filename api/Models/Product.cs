using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
    [Collection("products")]
    public class Product : BaseModel
    {
        public ObjectId VendorId { get; set; } = ObjectId.Empty;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Price { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public int CountInStock { get; set; } = 0;
    }
}