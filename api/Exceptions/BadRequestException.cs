using System.Net;

namespace api.Exceptions
{
    /// <summary>
    /// The BadRequestException class represents an exception that occurs when a bad request is made to the API.
    /// </summary>
    /// 
    /// <remarks>
    /// The BadRequestException class is a custom exception that inherits from the BaseException class.
    /// It is thrown when a bad request is made to the API, and includes the HTTP status code and a message.
    /// </remarks>
    public class BadRequestException(string message) : BaseException(message, HttpStatusCode.BadRequest, "Bad Request")
    {

    }
}