using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Utilities;

namespace api.DTOs.Models
{
    public class OrderDto
    {
        public string Id { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
        public string UserId { get; set; } = "";
        public OrderStatus Status { get; set; } = OrderStatus.Pending!;
        public List<Item> Products { get; set; } = [];
        public string DeliveryNote { get; set; } = "";
        public string DeliveryAddress { get; set; } = "";
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        public DateTime? ActualDeliveryDate { get; set; }
        public IEnumerable<string> VendorIds { get; set; } = [];

        public class Item
        {
            public string ProductId { get; set; } = "";
            public string VendorId { get; set; } = "";
            public int Quantity { get; set; } = 0;
            public OrderStatus Status { get; set; } = OrderStatus.Pending!;
            public string Name { get; set; } = "";
            public double Price { get; set; }
        }
    }
}