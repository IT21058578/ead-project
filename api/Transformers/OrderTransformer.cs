using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Models;
using MongoDB.Bson;

namespace api.Transformers
{
    public static class OrderTransformer
    {
        public static Order ToModel(this CreateOrderRequestDto request)
        {
            return new Order
            {
                UserId = new ObjectId(request.UserId),
                Status = request.Status,
                DeliveryAddress = request.DeliveryAddress,
                DeliveryDate = request.DeliveryDate,
                DeliveryNote = request.DeliveryNote,
                Products = request.Products.Select(item =>
                {
                    return new Order.Item
                    {
                        ProductId = new ObjectId(item.ProductId),
                        VendorId = new ObjectId(item.VendorId),
                        Name = item.Name,
                        Price = item.Price,
                        Status = item.Status,
                        Quantity = item.Quantity,
                    };
                }).ToList(),
            };
        }
    }
}