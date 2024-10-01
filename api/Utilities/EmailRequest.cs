using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Templates;

namespace api.Utilities
{
    public class EmailRequest<T> where T : ITemplateData
    {
        public string To { get; set; } = "";
        public string Subject { get; set; } = "";
        public string From { get; set; } = "";
        public EmailTemplate TemplateName { get; set; } = EmailTemplate.Registration;
        public T? TemplateData { get; set; }
    }
}