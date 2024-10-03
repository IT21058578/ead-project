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
    public static class NotificationTransformer
    {
        public static Notification ToModel(this CreateOrderCancellationRequestDto request)
        {
            return new Notification
            {
                OrderId = new ObjectId(request.OrderId),
                UserId = new ObjectId(request.UserId),
                Reason = request.Reason,
                Type = NotificationType.OrderCancellationRequest,
                Status = NotificationStatus.Unread,
            };
        }

        public static NotificationDto ToDto(this Notification model)
        {
            return new NotificationDto
            {
                Id = model.Id.ToString(),
                OrderId = model.OrderId.ToString(),
                UserId = model.UserId.ToString(),
                Reason = model.Reason,
                Type = model.Type,
                Status = model.Status,
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy.ToString(),
                UpdatedAt = model.UpdatedAt,
                UpdatedBy = model.UpdatedBy.ToString(),
                ProductId = model.ProductId.ToString(),
                Recipient = model.Recipient,
                AddresedBy = model.AddresedBy.ToString(),
            };
        }

        public static Notification ToModel(this UpdateNotificationRequestDto request, Notification model)
        {
            return new Notification
            {
                Id = model.Id,
                OrderId = model.OrderId,
                UserId = model.UserId,
                Reason = request.Reason,
                Status = request.Status,
                AddresedBy = new ObjectId(request.AddresedBy),
                Type = model.Type,
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = model.UpdatedBy,
                ProductId = model.ProductId,
                Recipient = model.Recipient,
            };
        }
    }
}