using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Utilities
{
    public class EmailRequest
    {
        public string To { get; set; } = "";
        public string Subject { get; set; } = "";
        public string From { get; set; } = "";
        public Template TemplateName { get; set; } = Template.Registration;
        public Dictionary<string, object> TemplateData { get; set; } = [];
        public enum Template
        {
            Registration,
            ResetPassword,
            PasswordChanged
        }
    }
}