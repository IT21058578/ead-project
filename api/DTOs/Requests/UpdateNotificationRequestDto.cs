
using api.Utilities;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The UpdateNotificationRequestDto class represents the data transfer object for updating a notification.
    /// </summary>
    /// 
    /// <remarks>
    /// The UpdateNotificationRequestDto class is used to transfer data for updating a notification.
    /// It contains properties for the reason, status, and addressed by fields of the notification.
    /// The reason property represents the reason for the notification.
    /// The status property represents the status of the notification, which can be Unread, Read, or Archived.
    /// The addressed by property represents the person who addressed the notification.
    /// </remarks>
    public class UpdateNotificationRequestDto
    {
        public string Reason { get; set; } = "";
        public NotificationStatus Status { get; set; } = NotificationStatus.Unread;
        public string? AddresedBy { get; set; }
    }
}