using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Models;
using MongoDB.Bson;

namespace api.Transformers
{
    public static class ProductTransformer
    {
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
    }
}