using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Utilities;

namespace api.DTOs.Models
{
    public class NotificationDto
    {
        public string Id { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
        public AppUserRole Recipient { get; set; } = AppUserRole.Customer;
        public NotificationType Type { get; set; } = NotificationType.UserApprovalRequest;
        public string UserId { get; set; } = "";
        public string? OrderId { get; set; }
        public string? ProductId { get; set; }
        public string Reason { get; set; } = "";
        public NotificationStatus Status { get; set; } = NotificationStatus.Unread;
        public string? AddresedBy { get; set; }
    }
}