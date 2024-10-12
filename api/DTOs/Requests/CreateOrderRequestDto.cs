
using System.ComponentModel.DataAnnotations;
using api.Annotations.Validation;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The CreateOrderRequestDto class represents the data required for creating an order.
    /// </summary>
    /// 
    /// <remarks>
    /// The CreateOrderRequestDto class is used to provide the necessary data for creating an order.
    /// It contains the UserId property which represents the ID of the user placing the order.
    /// It contains the VendorId property which represents the ID of the vendor from whom the order is being placed.
    /// It contains the Products property which represents the list of order items.
    /// It contains the DeliveryNote property which represents the delivery note for the order.
    /// It contains the DeliveryAddress property which represents the delivery address for the order.
    /// It contains the DeliveryDate property which represents the delivery date for the order.
    /// </remarks>
    public class CreateOrderRequestDto
    {
        [Required]
        [ValidObjectId]
        public string UserId { get; set; } = string.Empty;
        [Required]
        [MinLength(1)]
        public List<OrderItemDto> Products { get; set; } = [];
        [Required]
        public string DeliveryNote { get; set; } = "";
        [Required]
        public string DeliveryAddress { get; set; } = "";
        [Required]
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
    }
}