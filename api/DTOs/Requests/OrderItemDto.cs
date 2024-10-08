
using System.ComponentModel.DataAnnotations;
using api.Annotations.Validation;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The OrderItemDto class represents the data transfer object for an order item.
    /// </summary>
    /// 
    /// <remarks>
    /// The OrderItemDto class is used to transfer data related to an order item.
    /// It contains properties such as ProductId, VendorId, Quantity, Name, and Price.
    /// The ProductId and VendorId properties are required and must be valid object IDs.
    /// The Quantity property is required and must be a positive integer.
    /// The Name property is required and must have a minimum length of 1 and a maximum length of 100.
    /// The Price property is required and must be a positive decimal number.
    /// </remarks>
    public class OrderItemDto
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
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; } = "";
        [Required]
        [Range(0.01, double.MaxValue)]
        public double Price { get; set; }
    }
}