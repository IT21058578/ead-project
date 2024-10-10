using api.DTOs.Models;
using api.DTOs.Requests;
using api.Models;
using MongoDB.Bson;

namespace api.Transformers
{
	/// <summary>
	/// The ReviewTransformer class provides methods for transforming data related to reviews.
	/// </summary>
	/// 
	/// <remarks>
	/// The ReviewTransformer class contains static methods for transforming data between different representations of reviews.
	/// It includes methods for transforming a CreateReviewRequestDto to a Review model, a Review model to a ReviewDto,
	/// and an UpdateReviewRequestDto to a Review model.
	/// </remarks>
	public static class ReviewTransformer
	{
		// This is a method for transforming a CreateReviewRequestDto to a Review model
		public static Review ToModel(this CreateReviewRequestDto request)
		{
			return new Review
			{
				VendorId = new ObjectId(request.VendorId),
				ProductId = new ObjectId(request.ProductId),
				UserId = new ObjectId(request.UserId),
				Message = request.Message,
				Rating = request.Rating,
			};
		}

		// This is a method for transforming a Review model to a ReviewDto
		public static ReviewDto ToDto(this Review model)
		{
			return new ReviewDto
			{
				Id = model.Id.ToString(),
				VendorId = model.VendorId.ToString(),
				ProductId = model.ProductId.ToString(),
				UserId = model.UserId.ToString(),
				Message = model.Message,
				Rating = model.Rating,
				CreatedAt = model.CreatedAt,
				CreatedBy = model.CreatedBy.ToString(),
				UpdatedAt = model.UpdatedAt,
				UpdatedBy = model.UpdatedBy.ToString(),
			};
		}

		// This is a method for transforming an UpdateReviewRequestDto to a Review model
		public static Review ToModel(this UpdateReviewRequestDto request, Review model)
		{
			return new Review
			{
				Id = model.Id,
				VendorId = model.VendorId,
				ProductId = model.ProductId,
				UserId = model.UserId,
				Message = request.Message,
				Rating = request.Rating,
				CreatedAt = model.CreatedAt,
				CreatedBy = model.CreatedBy,
				UpdatedAt = DateTime.UtcNow,
				UpdatedBy = model.UpdatedBy,
			};
		}
	}
}