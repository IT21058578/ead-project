using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Models;
using api.Utilities;
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
                DeliveryAddress = request.DeliveryAddress,
                Status = OrderStatus.Pending,
                DeliveryDate = request.DeliveryDate,
                DeliveryNote = request.DeliveryNote,
                Products = request.Products.Select(item =>
                {
                    return new Order.Item
                    {
                        ProductId = new ObjectId(item.ProductId),
                        VendorId = new ObjectId(item.VendorId),
                        Status = OrderStatus.Pending,
                        Name = item.Name,
                        Price = item.Price,
                        Quantity = item.Quantity,
                    };
                }).ToList(),
            };
        }

        public static Order ToModel(this UpdateOrderRequestDto request)
        {
            return new Order
            {
                UserId = new ObjectId(request.UserId),
                DeliveryAddress = request.DeliveryAddress,
                Status = request.Status,
                DeliveryDate = request.DeliveryDate,
                DeliveryNote = request.DeliveryNote,
                Products = request.Products.Select(item =>
                {
                    return new Order.Item
                    {
                        ProductId = new ObjectId(item.ProductId),
                        VendorId = new ObjectId(item.VendorId),
                        Status = OrderStatus.Pending,
                        Name = item.Name,
                        Price = item.Price,
                        Quantity = item.Quantity,
                    };
                }).ToList(),
            };
        }
    }
}