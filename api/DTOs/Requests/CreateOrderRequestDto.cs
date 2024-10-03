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
        [MinLength(1)]
        public List<OrderItemDto> Products { get; set; } = [];
        [MinLength(300)]
        public string DeliveryNote { get; set; } = "";
        [Required]
        [MinLength(300)]
        public string DeliveryAddress { get; set; } = "";
        [Required]
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
    }
}