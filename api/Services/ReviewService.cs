
using api.DTOs.Requests;
using api.Exceptions;
using api.Models;
using api.Repositories;
using api.Transformers;
using api.Utilities;

namespace api.Services
{
	/// <summary>
	/// The ReviewService class provides methods for managing reviews.
	/// </summary>
	/// 
	/// <remarks>
	/// The ReviewService class is responsible for creating, updating, deleting, and retrieving reviews.
	/// It also provides methods for searching reviews and updating the ratings of vendors and products.
	/// </remarks>
	public class ReviewService(
		ReviewRepository reviewRepository,
		ILogger<ReviewService> logger,
		ProductService productService,
		UserService userService)
	{
		private readonly ILogger<ReviewService> _logger = logger;
		private readonly ReviewRepository _reviewRepository = reviewRepository;
		private readonly ProductService _productService = productService;
		private readonly UserService _userService = userService;

		// This method creates a review
		public Review CreateReview(CreateReviewRequestDto request)
		{
			_logger.LogInformation("Creating review");
			var review = request.ToModel();
			ValidateReviewAndThrowIfInvalid(review);
			var savedReview = _reviewRepository.Add(review);
			_logger.LogInformation("Review created with id {id}", savedReview.Id);
			_ = UpdateProductRating(request.ProductId);
			_ = UpdateVendorRating(request.VendorId);
			return savedReview;
		}

		// This method updates a vendor rating
		public async Task UpdateVendorRating(string vendorId)
		{
			_logger.LogInformation("Updating vendor rating for vendor {vendorId}", vendorId);
			var reviews = _reviewRepository.GetByVendorId(vendorId);
			var vendorRating = reviews.Average(r => r.Rating);
			await _userService.UpdateRating(vendorId, vendorRating);
			_logger.LogInformation("Vendor rating updated to {rating}", vendorRating);
		}

		// This method updates a product rating
		public async Task UpdateProductRating(string productId)
		{
			_logger.LogInformation("Updating product rating for product {productId}", productId);
			var reviews = _reviewRepository.GetByProductId(productId);
			var productRating = reviews.Average(r => r.Rating);
			await _productService.UpdateRating(productId, productRating);
			_logger.LogInformation("Product rating updated to {rating}", productRating);
		}

		// This method deletes a review
		public void DeleteReview(string id)
		{
			_logger.LogInformation("Deleting review {id}", id);
			var review = _reviewRepository.Delete(id);
			_logger.LogInformation("Review deleted");
			_ = UpdateProductRating(review.ProductId.ToString());
			_ = UpdateVendorRating(review.VendorId.ToString());
		}

		// This method gets a review
		public Review GetReview(string id)
		{
			_logger.LogInformation("Getting review {id}", id);
			var review = _reviewRepository.GetById(id) ?? throw new NotFoundException($"Review with id {id} not found");
			_logger.LogInformation("Review found with id {id}", review.Id);
			return review;
		}

		// This method searches reviews
		public Page<Review> SearchReviews(PageRequest<Review> request)
		{
			_logger.LogInformation("Searching reviews with page {page} and page size {pageSize}", request.Page, request.PageSize);
			var reviews = _reviewRepository.GetPage(request);
			_logger.LogInformation("Found {count} reviews", reviews.Data.Count());
			return reviews;
		}

		// This method updates a review
		public Review UpdateReview(string id, UpdateReviewRequestDto request)
		{
			_logger.LogInformation("Updating review {id}", id);
			var existingReview = GetReview(id);
			var review = request.ToModel(existingReview);
			ValidateReviewAndThrowIfInvalid(review);
			var updatedReview = _reviewRepository.Update(review);
			_logger.LogInformation("Review updated with id {id}", updatedReview.Id);
			_ = UpdateProductRating(review.ProductId.ToString());
			_ = UpdateVendorRating(review.VendorId.ToString());
			return updatedReview;
		}

		// This method validates a review and throws an exception if the review is invalid
		public void ValidateReviewAndThrowIfInvalid(Review review)
		{
			if (!_userService.IsUserValid(review.UserId.ToString()))
			{
				throw new NotFoundException($"User with id {review.UserId} not found");
			};
			if (!_userService.IsUserValid(review.VendorId.ToString()))
			{
				throw new NotFoundException($"Vendor with id {review.VendorId} not found");
			};
			if (!_productService.IsProductValid(review.ProductId.ToString()))
			{
				throw new NotFoundException($"Product with id {review.ProductId} not found");
			};
		}
	}
}