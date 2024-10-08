using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace api.Utilities
{
    /// <summary>
    /// The ProblemDetails class represents a standardized format for returning error details in an API response.
    /// </summary>
    /// 
    /// <remarks>
    /// The ProblemDetails class is used to encapsulate information about an error that occurred during the processing of an API request.
    /// It contains properties for the title, status code, detail message, error type, trace ID, and additional data related to the error.
    /// </remarks>
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