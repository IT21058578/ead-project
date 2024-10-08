using System.Net;

namespace api.Exceptions
{
    /// <summary>
    /// The InternalServerErrorException class represents an exception that is thrown when an internal server error occurs.
    /// </summary>
    /// 
    /// <remarks>
    /// This exception is typically thrown when there is an unexpected error on the server side that prevents the request from being processed successfully.
    /// The message parameter should provide additional information about the error.
    /// The HttpStatusCode.InternalServerError status code is used to indicate that an internal server error has occurred.
    /// The "Internal Server Error" reason phrase is used to provide a human-readable description of the error.
    /// </remarks>
    public class InternalServerErrorException(string message) : BaseException(message, HttpStatusCode.InternalServerError, "Internal Server Error")
    {

    }
}