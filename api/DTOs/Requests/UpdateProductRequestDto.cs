using System.ComponentModel.DataAnnotations;
using api.Annotations.Validation;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The UpdateProductRequestDto class represents the data transfer object for updating a product.
    /// </summary>
    /// 
    /// <remarks>
    /// The UpdateProductRequestDto class is used to transfer data for updating a product.
    /// It contains properties for the vendor ID, name, description, category, price, 
    /// active status, count in stock, low stock threshold, and image URL of the product.
    /// </remarks>
    public class UpdateProductRequestDto
    {
        [Required]
        [ValidObjectId]
        public string VendorId { get; set; } = string.Empty;
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; } = "";
        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Description { get; set; } = "";
        [Required]
        public string Category { get; set; } = "";
        [Required]
        [Range(0.01, double.MaxValue)]
        public double Price { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        [Required]
        [Range(0, int.MaxValue)]
        public int CountInStock { get; set; } = 0;
        [Required]
        [Range(0, int.MaxValue)]
        public int LowStockThreshold { get; set; } = 10;
        [Url]
        [Required]
        public string ImageUrl { get; set; } = "";
    }
}