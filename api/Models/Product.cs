using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
    /// <summary>
    /// The Product class represents a product in the API.
    /// </summary>
    /// 
    /// <remarks>
    /// The Product class inherits from the BaseModel class and contains properties specific to a product.
    /// These properties include the VendorId, Name, Description, Category, Price, IsActive, CountInStock, LowStockThreshold, Rating, and ImageUrl properties.
    /// </remarks>
    [Collection("products")]
    public class Product : BaseModel
    {
        public ObjectId VendorId { get; set; } = ObjectId.Empty;
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