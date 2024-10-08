using api.DTOs.Models;
using api.DTOs.Requests;
using api.Models;
using MongoDB.Bson;

namespace api.Transformers
{
    /// <summary>
    /// The ProductTransformer class provides methods for transforming data between different representations of a product.
    /// </summary>
    /// 
    /// <remarks>
    /// The ProductTransformer class contains static methods for transforming a CreateProductRequestDto to a Product model,
    /// transforming a Product model to a ProductDto, and transforming an UpdateProductRequestDto to a Product model.
    /// These methods are used to convert data between different layers of an application, such as the API layer and the data access layer.
    /// </remarks>
    public static class ProductTransformer
    {
        // This is a method for transforming a CreateProductRequestDto to a Product model
        public static Product ToModel(this CreateProductRequestDto request)
        {
            return new Product
            {
                VendorId = new ObjectId(request.VendorId),
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                Price = request.Price,
                IsActive = request.IsActive,
                CountInStock = request.CountInStock,
                LowStockThreshold = request.LowStockThreshold,
            };
        }

        // This is a method for transforming a Product model to a ProductDto
        public static ProductDto ToDto(this Product model)
        {
            return new ProductDto
            {
                Id = model.Id.ToString(),
                VendorId = model.VendorId.ToString(),
                Name = model.Name,
                Description = model.Description,
                Category = model.Category,
                Price = model.Price,
                IsActive = model.IsActive,
                CountInStock = model.CountInStock,
                LowStockThreshold = model.LowStockThreshold,
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy.ToString(),
                UpdatedAt = model.UpdatedAt,
                UpdatedBy = model.UpdatedBy.ToString(),
                ImageUrl = model.ImageUrl,
                Rating = model.Rating,
            };
        }

        // This is a method for transforming an UpdateProductRequestDto to a Product model
        public static Product ToModel(this UpdateProductRequestDto request, Product model)
        {
            return new Product
            {
                Id = model.Id,
                VendorId = new ObjectId(request.VendorId),
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                Price = request.Price,
                IsActive = request.IsActive,
                CountInStock = request.CountInStock,
                LowStockThreshold = request.LowStockThreshold,
                ImageUrl = request.ImageUrl,
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedAt = model.UpdatedAt,
                UpdatedBy = model.UpdatedBy,
                Rating = model.Rating,
            };
        }
    }
}