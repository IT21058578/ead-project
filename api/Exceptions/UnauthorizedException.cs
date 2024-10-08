using System.Net;

namespace api.Exceptions
{
    /// <summary>
    /// The UnauthorizedException class represents an exception that is thrown when a user is not authorized to perform a certain action.
    /// </summary>
    /// 
    /// <remarks>
    /// This exception is typically thrown when a user tries to access a resource or perform an operation without the necessary authorization.
    /// The exception message should provide more details about the specific authorization issue.
    /// </remarks>
    public class UnauthorizedException(string message) : BaseException(message, HttpStatusCode.Unauthorized, "Unauthorized")
    {

    }
}