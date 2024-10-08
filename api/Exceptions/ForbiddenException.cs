using System.Net;

namespace api.Exceptions
{
    /// <summary>
    /// The ForbiddenException class represents an exception that is thrown when a forbidden operation is attempted.
    /// </summary>
    /// 
    /// <remarks>
    /// This exception is typically thrown when a user tries to perform an operation that they do not have permission to perform.
    /// The ForbiddenException class inherits from the BaseException class and sets the HttpStatusCode to Forbidden.
    /// </remarks>
    public class ForbiddenException(string message) : BaseException(message, HttpStatusCode.Forbidden, "Forbidden")
    {

    }
}