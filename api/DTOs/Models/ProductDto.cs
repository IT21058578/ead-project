namespace api.DTOs.Models
{
    /// <summary>
    /// The ProductDto class represents the data transfer object for a product.
    /// </summary>
    /// 
    /// <remarks>
    /// The ProductDto class is used to transfer data for a product.
    /// It contains properties for the ID, created by, created at, updated by, updated at,
    /// vendor ID, name, description, category, price, active status, count in stock,
    /// low stock threshold, rating, and image URL of the product.
    /// </remarks>
    public class ProductDto
    {
        public string Id { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
        public string VendorId { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public double Price { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public int CountInStock { get; set; } = 0;
        public int LowStockThreshold { get; set; } = 10;
        public double Rating { get; set; } = 0;
        public string? ImageUrl { get; set; }
    }
}