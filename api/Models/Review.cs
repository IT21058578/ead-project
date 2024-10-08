using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
	/// <summary>
	/// The Review class represents a review for a product in the API.
	/// </summary>
	/// 
	/// <remarks>
	/// The Review class is a derived class of the BaseModel class and contains properties specific to a review.
	/// These properties include the VendorId, ProductId, UserId, Message, and Rating properties.
	/// </remarks>
	[Collection("reviews")]
	public class Review : BaseModel
	{
		public ObjectId VendorId { get; set; } = ObjectId.Empty!;
		public ObjectId ProductId { get; set; } = ObjectId.Empty!;
		public ObjectId UserId { get; set; } = ObjectId.Empty!;
		public string Message { get; set; } = "";
		public double Rating { get; set; } = 0;
	}
}