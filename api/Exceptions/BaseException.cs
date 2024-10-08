using System.Net;

namespace api.Exceptions
{
    /// <summary>
    /// The BaseException class is a custom exception class that serves as the base for all exceptions in the API.
    /// </summary>
    /// 
    /// <remarks>
    /// The BaseException class contains common properties that are inherited by all exceptions in the API.
    /// These properties include the Code and Title properties, which represent the HTTP status code and title of the exception, respectively.
    /// </remarks>
    public class BaseException(string message, HttpStatusCode code, string title) : Exception(message)
    {
        public HttpStatusCode Code { get; } = code;
        public string Title { get; } = title;
    }
}