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
        public string Template { get; set; } = "";
        public Dictionary<string, object> TemplateData { get; set; } = [];
    }
}