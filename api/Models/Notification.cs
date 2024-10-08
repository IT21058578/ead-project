using api.Utilities;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
    /// <summary>
    /// The Notification class represents a notification in the API.
    /// </summary>
    /// 
    /// <remarks>
    /// The Notification class is a derived class of the BaseModel class and inherits its common properties.
    /// It contains properties such as Recipient, Type, UserId, OrderId, ProductId, Reason, Status, and AddresedBy.
    /// </remarks>
    [Collection("notifications")]
    public class Notification : BaseModel
    {
        public AppUserRole Recipient { get; set; } = AppUserRole.Customer;
        public NotificationType Type { get; set; } = NotificationType.UserApprovalRequest;
        public ObjectId UserId { get; set; } = ObjectId.Empty;
        public ObjectId? OrderId { get; set; } = ObjectId.Empty;
        public ObjectId? ProductId { get; set; } = ObjectId.Empty;
        public string Reason { get; set; } = "";
        public NotificationStatus Status { get; set; } = NotificationStatus.Unread;
        public ObjectId? AddresedBy { get; set; }
    }
}
