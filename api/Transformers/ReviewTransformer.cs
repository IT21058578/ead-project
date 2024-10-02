using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Models;
using api.DTOs.Requests;
using api.Models;
using MongoDB.Bson;

namespace api.Transformers
{
	public static class ReviewTransformer
	{
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
	}
}