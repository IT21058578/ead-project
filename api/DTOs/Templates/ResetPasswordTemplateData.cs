using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Templates
{
    public class ResetPasswordTemplateData : ITemplateData
    {
        public string FirstName { get; set; } = "";
        public string ResetPasswordLink { get; set; } = "";
        public string ResetPasswordCode { get; set; } = "";
    }
}