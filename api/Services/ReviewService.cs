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
	public class ReviewService(ReviewRepository reviewRepository, ILogger<ReviewService> logger, ProductService productService, UserService userService)
	{
		private readonly ILogger<ReviewService> _logger = logger;
		private readonly ReviewRepository _reviewRepository = reviewRepository;
		private readonly ProductService _productService = productService;
		private readonly UserService _userService = userService;

		public Review CreateReview(CreateReviewRequestDto request)
		{
			_logger.LogInformation("Creating review");
			var review = request.ToModel();
			var savedReview = _reviewRepository.Add(review);
			_logger.LogInformation("Review created with id {id}", savedReview.Id);
			_ = UpdateProductRating(request.ProductId);
			_ = UpdateVendorRating(request.VendorId);
			return savedReview;
		}

		public async Task UpdateVendorRating(string vendorId)
		{
			_logger.LogInformation("Updating vendor rating for vendor {vendorId}", vendorId);
			var reviews = _reviewRepository.GetByVendorId(vendorId);
			var vendorRating = reviews.Average(r => r.Rating);
			await _userService.UpdateRating(vendorId, vendorRating);
			_logger.LogInformation("Vendor rating updated to {rating}", vendorRating);
		}

		public async Task UpdateProductRating(string productId)
		{
			_logger.LogInformation("Updating product rating for product {productId}", productId);
			var reviews = _reviewRepository.GetByProductId(productId);
			var productRating = reviews.Average(r => r.Rating);
			await _productService.UpdateRating(productId, productRating);
			_logger.LogInformation("Product rating updated to {rating}", productRating);
		}

		public void DeleteReview(string id)
		{
			_logger.LogInformation("Deleting review {id}", id);
			var review = _reviewRepository.Delete(id);
			_logger.LogInformation("Review deleted");
			_ = UpdateProductRating(review.ProductId.ToString());
			_ = UpdateVendorRating(review.VendorId.ToString());
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
			_ = UpdateProductRating(request.ProductId);
			_ = UpdateVendorRating(request.VendorId);
			return updatedReview;
		}
	}
}