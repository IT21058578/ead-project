using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
    [Collection("orders")]
    public class Order : BaseModel
    {
        public ObjectId UserId { get; set; } = ObjectId.Empty;
        public string Status { get; set; } = null!;
        public List<Item> Products { get; set; } = [];
        public string DeliveryNote { get; set; } = null!;
        public string DeliveryAddress { get; set; } = null!;
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        public DateTime? ActualDeliveryDate { get; set; } = null!;
        public IEnumerable<string> VendorIds { get; set; } = [];
        // Assumption: Payments are always done when Order is created.

        public class Item
        {
            public ObjectId ProductId { get; set; } = ObjectId.Empty;
            public ObjectId VendorId { get; set; } = ObjectId.Empty;
            public int Quantity { get; set; } = 0;
            public string Status { get; set; } = null!;
            public string Name { get; set; } = null!;
            public double Price { get; set; }
        }
    }
}