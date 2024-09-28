using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace api.Models
{
    public class CancellationRequest : BaseModel
    {
        public ObjectId OrderId { get; set; } = ObjectId.Empty;
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public ObjectId? AddresedBy { get; set; }
    }
}
