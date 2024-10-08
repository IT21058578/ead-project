using System.Net;

namespace api.Exceptions
{
    /// <summary>
    /// The NotFoundException class represents an exception that is thrown when a resource is not found.
    /// </summary>
    /// 
    /// <remarks>
    /// This exception is typically thrown when a requested resource cannot be found in the API.
    /// The exception message should provide additional details about the resource that was not found.
    /// </remarks>
    public class NotFoundException(string message) : BaseException(message, HttpStatusCode.NotFound, "Not Found")
    {

    }
}