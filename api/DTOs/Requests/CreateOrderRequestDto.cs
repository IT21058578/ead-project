using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace api.DTOs.Requests
{
    public class CreateOrderRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public string VendorId { get; set; } = string.Empty;
        public string Status { get; set; } = null!;
        public List<Item> Products { get; set; } = [];
        public string DeliveryNote { get; set; } = null!;
        public string DeliveryAddress { get; set; } = null!;
        public DateTime DeliveryDate { get; set; } = DateTime.Now;

        public class Item
        {
            public string ProductId { get; set; } = string.Empty;
            public string VendorId { get; set; } = string.Empty;
            public int Quantity { get; set; } = 0;
            public string Status { get; set; } = null!;
            public string Name { get; set; } = null!;
            public double Price { get; set; }
        }
    }
}