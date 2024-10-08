
using System.ComponentModel.DataAnnotations;
using api.Annotations.Validation;

namespace api.DTOs.Requests
{
	/// <summary>
	/// The CreateReviewRequestDto class represents the data transfer object for creating a review.
	/// </summary>
	/// 
	/// <remarks>
	/// The CreateReviewRequestDto class is used to transfer data for creating a review.
	/// It contains properties for the vendor ID, product ID, user ID, message, and rating.
	/// The vendor ID, product ID, and user ID are required fields and must be valid object IDs.
	/// The message must have a minimum length of 1 character and a maximum length of 500 characters.
	/// The rating must be a number between 0 and 5.
	/// </remarks>
	public class CreateReviewRequestDto
	{
		[Required]
		[ValidObjectId]
		public string VendorId { get; set; } = string.Empty!;
		[Required]
		[ValidObjectId]
		public string ProductId { get; set; } = string.Empty!;
		[Required]
		[ValidObjectId]
		public string UserId { get; set; } = string.Empty!;
		[Required]
		[MinLength(1)]
		[MaxLength(500)]
		public string Message { get; set; } = "";
		[Required]
		[Range(0, 5)]
		public double Rating { get; set; } = 0;
	}
}