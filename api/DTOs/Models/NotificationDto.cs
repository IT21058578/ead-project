
using api.Utilities;

namespace api.DTOs.Models
{

    /// <summary>
    /// The NotificationDto class represents the data transfer object for a notification.
    /// </summary>
    /// 
    /// <remarks>
    /// The NotificationDto class is used to transfer data for a notification.
    /// It contains properties for the ID, creator, creation timestamp, updater, update timestamp,
    /// recipient role, notification type, user ID, order ID, product ID, reason, status, and addressed by.
    /// </remarks>
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