using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Configuratons
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public int AccessExpiryMs { get; set; } = 0;
        public string AccessSecret { get; set; } = "";
        public int RefreshExpiryMs { get; set; } = 0;
        public string RefreshSecret { get; set; } = "";
    }
}