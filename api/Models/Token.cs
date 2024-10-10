using api.Utilities;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
    /// <summary>
    /// The Token class represents a token used for authentication and authorization in the API.
    /// </summary>
    /// 
    /// <remarks>
    /// The Token class inherits from the BaseModel class and contains properties for the token code, email, purpose, and status.
    /// The Code property represents the unique code associated with the token.
    /// The Email property represents the email address associated with the token.
    /// The Purpose property represents the purpose of the token, such as registration or password reset.
    /// The Status property represents the status of the token, such as active or expired.
    /// </remarks>
    [Collection("tokens")]
    public class Token : BaseModel
    {
        public string Code { get; set; } = "";
        public string Email { get; set; } = "";
        public TokenPurpose Purpose { get; set; } = TokenPurpose.Registration;
        public TokenStatus Status { get; set; } = TokenStatus.Active;
    }
}