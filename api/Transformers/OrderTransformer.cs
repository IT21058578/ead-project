using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Models;
using api.DTOs.Requests;
using api.Models;
using api.Utilities;
using MongoDB.Bson;

namespace api.Transformers
{
    public static class OrderTransformer
    {
        // This is a method for transforming a CreateOrderRequestDto to an Order model
        public static Order ToModel(this CreateOrderRequestDto request)
        {
            return new Order
            {
                UserId = new ObjectId(request.UserId),
                DeliveryAddress = request.DeliveryAddress,
                VendorIds = request.Products.Select(item => item.VendorId).ToList(),
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

        // This is a method for transforming an UpdateOrderRequestDto to an Order model
        public static Order ToModel(this UpdateOrderRequestDto request)
        {
            return new Order
            {
                UserId = new ObjectId(request.UserId),
                DeliveryAddress = request.DeliveryAddress,
                Status = request.Status,
                DeliveryDate = request.DeliveryDate,
                DeliveryNote = request.DeliveryNote,
                VendorIds = request.Products.Select(item => item.VendorId).ToList(),
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

        // This is a method for transforming an Order model to an OrderDto
        public static OrderDto ToDto(this Order model)
        {
            return new OrderDto
            {
                Id = model.Id.ToString(),
                UserId = model.UserId.ToString(),
                DeliveryAddress = model.DeliveryAddress,
                Status = model.Status,
                DeliveryDate = model.DeliveryDate,
                DeliveryNote = model.DeliveryNote,
                Products = model.Products.Select(item =>
                {
                    return new OrderDto.Item
                    {
                        ProductId = item.ProductId.ToString(),
                        VendorId = item.VendorId.ToString(),
                        Status = item.Status,
                        Name = item.Name,
                        Price = item.Price,
                        Quantity = item.Quantity,
                    };
                }).ToList(),
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy.ToString(),
                UpdatedAt = model.UpdatedAt,
                UpdatedBy = model.UpdatedBy.ToString(),
                ActualDeliveryDate = model.ActualDeliveryDate,
                VendorIds = model.Products.Select(item => item.VendorId.ToString()).Distinct().ToList(),
            };
        }
    }
}