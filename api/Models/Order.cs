using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Order : IMongoModel
    {
        public string? Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Status { get; set; } = null!;
        public List<SimpleProduct> Products { get; set; } = [];
        public string DeliveryNote { get; set; } = null!;
        public string DeliveryAddress { get; set; } = null!;
        public string DeliveryDate { get; set; } = null!;
    
        public class SimpleProduct {
            public string Id { get; set; } = null!;
            public string Name { get; set; } = null!;
            public double Price { get; set; }
        }
    }
}