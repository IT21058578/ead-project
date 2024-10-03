using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace api.Exceptions
{
    public class BaseException(string message, HttpStatusCode code, string title) : Exception(message)
    {
        public HttpStatusCode Code { get; } = code;
        public string Title { get; } = title;
    }
}