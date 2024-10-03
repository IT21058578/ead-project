using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace api.Utilities
{
    public class ProblemDetails
    {
        public string Title { get; set; } = "";
        public int Status { get; set; } = 500;
        public string Detail { get; set; } = "";
        public string Type { get; set; } = "";
        public string TraceId { get; set; } = "";
        public object Data = new { };
    }
}