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
    }
}