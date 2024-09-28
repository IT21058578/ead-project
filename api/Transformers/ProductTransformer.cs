using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using MongoDB.Bson;

namespace api.Transformers
{
    public static class ProductTransformer
    {
        public static Models.Product ToModel(this CreateProductRequestDto request)
        {
            return new Models.Product
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