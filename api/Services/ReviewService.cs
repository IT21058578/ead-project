using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Models;
using api.Repositories;
using api.Transformers;
using api.Utilities;

namespace api.Services
{
	public class ReviewService(ReviewRepository reviewRepository, ILogger<ReviewService> logger)
	{
		private readonly ILogger<ReviewService> _logger = logger;
		private readonly ReviewRepository _reviewRepository = reviewRepository;

		public Review CreateReview(CreateReviewRequestDto request)
		{
			_logger.LogInformation("Creating review");
			var review = request.ToModel();
			var savedReview = _reviewRepository.Add(review);
			_logger.LogInformation("Review created with id {id}", savedReview.Id);
			return savedReview;
		}

		public void DeleteReview(string id)
		{
			_logger.LogInformation("Deleting review {id}", id);
			_reviewRepository.Delete(id);
			_logger.LogInformation("Review deleted");
		}

		public Review GetReview(string id)
		{
			_logger.LogInformation("Getting review {id}", id);
			var review = _reviewRepository.GetById(id) ?? throw new Exception("Review not found");
			_logger.LogInformation("Review found with id {id}", review.Id);
			return review;
		}

		public Page<Review> SearchReviews(PageRequest<Review> request)
		{
			_logger.LogInformation("Searching reviews with page {page} and page size {pageSize}", request.Page, request.PageSize);
			var reviews = _reviewRepository.GetPage(request);
			_logger.LogInformation("Found {count} reviews", reviews.Data.Count());
			return reviews;
		}

		public Review UpdateReview(string id, CreateReviewRequestDto request)
		{
			_logger.LogInformation("Updating review {id}", id);
			var review = request.ToModel();
			var updatedReview = _reviewRepository.Update(review);
			_logger.LogInformation("Review updated with id {id}", updatedReview.Id);
			return updatedReview;
		}
	}
}