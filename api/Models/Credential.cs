using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
    [Collection("credentials")]
    public class Credential : BaseModel
    {
        public string Password { get; set; } = null!;
    }
}