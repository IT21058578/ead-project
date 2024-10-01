using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Utilities;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
    [Collection("tokens")]
    public class Token : BaseModel
    {
        public string Code { get; set; } = "";
        public string Email { get; set; } = "";
        public TokenPurpose Purpose { get; set; } = TokenPurpose.Registration;
        public TokenStatus Status { get; set; } = TokenStatus.Active;
    }
}