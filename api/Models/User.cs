using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
    [Collection("users")]
    public class User : BaseModel
    {
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool IsVerified { get; set; } = false;
        public bool IsApproved { get; set; } = false;

    }
}