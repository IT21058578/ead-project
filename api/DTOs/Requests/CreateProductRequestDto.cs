
using System.ComponentModel.DataAnnotations;
using api.Annotations.Validation;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The CreateProductRequestDto class represents the data required for creating a new product.
    /// </summary>
    /// 
    /// <remarks>
    /// The CreateProductRequestDto class is used to provide the necessary data for creating a new product.
    /// It contains the VendorId property which represents the ID of the vendor.
    /// It contains the Name property which represents the name of the product.
    /// It contains the Description property which represents the description of the product.
    /// It contains the Category property which represents the category of the product.
    /// It contains the Price property which represents the price of the product.
    /// It contains the IsActive property which indicates whether the product is active or not.
    /// It contains the CountInStock property which represents the count of the product in stock.
    /// It contains the LowStockThreshold property which represents the low stock threshold of the product.
    /// It contains the ImageUrl property which represents the URL of the product image.
    /// </remarks>
    public class CreateProductRequestDto
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
        [Required]
        public string ImageUrl { get; set; } = "";
    }
}