using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Templates
{
    public class PasswordChangedTemplateData : ITemplateData
    {
        public string FirstName { get; set; } = "";
    }
}