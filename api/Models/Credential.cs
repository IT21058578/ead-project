using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
    /// <summary>
    /// The Credential class represents a user's credential in the API.
    /// </summary>
    /// 
    /// <remarks>
    /// The Credential class represents a user's credential in the API.
    /// It contains properties such as UserId and Password.
    /// </remarks>
    [Collection("credentials")]
    public class Credential : BaseModel
    {
        public ObjectId UserId { get; set; } = ObjectId.Empty;
        public string Password { get; set; } = "";
    }
}