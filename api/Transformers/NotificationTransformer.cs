using api.DTOs.Models;
using api.DTOs.Requests;
using api.Models;
using api.Utilities;
using MongoDB.Bson;

namespace api.Transformers
{
    /// <summary>
    /// The NotificationTransformer class provides methods for transforming notification objects.
    /// </summary>
    /// 
    /// <remarks>
    /// The NotificationTransformer class contains static methods for transforming different types of notification objects.
    /// It includes methods for transforming a CreateOrderCancellationRequestDto to a Notification model,
    /// transforming a Notification model to a NotificationDto, and transforming an UpdateNotificationRequestDto to a Notification model.
    /// </remarks>
    public static class NotificationTransformer
    {
        // This is a method for transforming a CreateOrderCancellationRequestDto to a Notification model
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

        // This is a method for transforming a Notification model to a NotificationDto
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

        // This is a method for transforming a UpdateNotificationRequestDto to a Notification model
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