using System.Net;

namespace api.Exceptions
{
    /// <summary>
    /// The ConflictException class represents an exception that is thrown when a conflict occurs in the API.
    /// </summary>
    /// 
    /// <remarks>
    /// This exception is typically thrown when there is a conflict between the current state of a resource and the requested operation.
    /// The ConflictException class inherits from the BaseException class and sets the HttpStatusCode to Conflict.
    /// </remarks>
    public class ConflictException(string message) : BaseException(message, HttpStatusCode.Conflict, "Conflict")
    {

    }
}