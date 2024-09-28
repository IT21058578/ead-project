using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace api.Models
{
    public class Notification : BaseModel
    {
        public string Recipient { get; set; } = null!; // VENDOR, CSR_REPRSENTATIVE, CUSTOMERS
        public string Type { get; set; } = null!; // USER_APPROVAL_REQUEST, ORDER_CANCELLATION_REQUEST, LOW_STOCK_NOTIFICATION, ORDER_CANCELLED_NOTIFICATION, ORDER_COMPLETED_NOTIFICATION
        public ObjectId UserId { get; set; } = ObjectId.Empty;
        public ObjectId? OrderId { get; set; } = ObjectId.Empty;
        public ObjectId? ProductId { get; set; } = ObjectId.Empty;
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public ObjectId? AddresedBy { get; set; }
    }
}
