
using System.ComponentModel.DataAnnotations;
using api.Annotations.Validation;
using api.Utilities;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The UpdateOrderRequestDto class represents the data transfer object for updating an order.
    /// </summary>
    /// 
    /// <remarks>
    /// The UpdateOrderRequestDto class is used to transfer data for updating an order.
    /// It contains properties for the user ID, vendor ID, order status, products, delivery note,
    /// delivery address, and delivery date.
    /// </remarks>
    public class UpdateOrderRequestDto
    {
        [Required]
        [ValidObjectId]
        public string UserId { get; set; } = string.Empty;
        [Required]
        [ValidObjectId]
        public string VendorId { get; set; } = string.Empty;
        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        [Required]
        [MinLength(1)]
        public List<OrderItemDto> Products { get; set; } = [];
        public string DeliveryNote { get; set; } = "";
        [Required]
        [MinLength(300)]
        public string DeliveryAddress { get; set; } = "";
        [Required]
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
    }
}