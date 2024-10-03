using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Utilities;

namespace api.DTOs.Requests
{
    public class UpdateNotificationRequestDto
    {
        public string Reason { get; set; } = "";
        public NotificationStatus Status { get; set; } = NotificationStatus.Unread;
        public string? AddresedBy { get; set; }
    }
}