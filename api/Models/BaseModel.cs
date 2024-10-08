using MongoDB.Bson;

namespace api.Models
{
    /// <summary>
    /// The BaseModel class is an abstract class that serves as the base for all models in the API.
    /// </summary>
    /// 
    /// <remarks>
    /// The BaseModel class contains common properties that are inherited by all models in the API.
    /// These properties include the Id, CreatedBy, CreatedAt, UpdatedBy, and UpdatedAt properties.
    /// </remarks>
    public abstract class BaseModel
    {
        public ObjectId Id { get; set; }
        public ObjectId CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public ObjectId UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}