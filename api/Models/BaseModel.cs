using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models
{
    public class BaseModel
    {
        public ObjectId Id { get; set; }
        public ObjectId CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public ObjectId UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}