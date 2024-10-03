using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Utilities;
using MongoDB.Bson;

namespace api.Models
{
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
