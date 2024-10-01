using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Templates
{
    public class RegisterTemplateData : ITemplateData
    {
        public string FirstName { get; set; } = "";
        public string VerificationCode { get; set; } = "";
        public string VerificationLink { get; set; } = "";
    }
}