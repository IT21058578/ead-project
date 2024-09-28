using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
	}
}