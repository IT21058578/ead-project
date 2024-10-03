using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
    [Collection("credentials")]
    public class Credential : BaseModel
    {
        public ObjectId UserId { get; set; } = ObjectId.Empty;
        public string Password { get; set; } = "";
    }
}