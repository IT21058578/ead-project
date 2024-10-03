using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Utilities;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
    [Collection("users")]
    public class User : BaseModel
    {
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public AppUserRole Role { get; set; } = AppUserRole.Customer;
        public bool IsVerified { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public double Rating { get; set; } = 0.0;
    }
}