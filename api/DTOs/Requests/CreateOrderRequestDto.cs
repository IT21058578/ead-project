using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Annotations.Validation;
using MongoDB.Bson;

namespace api.DTOs.Requests
{
    public class CreateOrderRequestDto
    {
        [Required]
        [ValidObjectId]
        public string UserId { get; set; } = string.Empty;
        [Required]
        [ValidObjectId]
        public string VendorId { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = null!;
        [Required]
        [MinLength(1)]
        public List<Item> Products { get; set; } = [];
        [MinLength(300)]
        public string DeliveryNote { get; set; } = null!;
        [Required]
        [MinLength(300)]
        public string DeliveryAddress { get; set; } = null!;
        [Required]
        public DateTime DeliveryDate { get; set; } = DateTime.Now;

        public class Item
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
            public string Status { get; set; } = null!;
            [Required]
            [MinLength(1)]
            [MaxLength(100)]
            public string Name { get; set; } = null!;
            [Required]
            [Range(0.01, double.MaxValue)]
            public double Price { get; set; }
        }
    }
}