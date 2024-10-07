using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Models;
using api.DTOs.Requests;
using api.Models;
using api.Services;
using api.Transformers;
using api.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers.V1
{
    [Route("api/v1/reviews")]
    [ApiController]
    public class ReviewController(ILogger<ReviewController> logger, ReviewService reviewService) : ControllerBase
    {
        private readonly ILogger<ReviewController> _logger = logger;
        private readonly ReviewService _reviewService = reviewService;

        // This is an endpoint exposed for getting a review
        [HttpGet("{id}")]
        public IActionResult GetReview([FromRoute] string id)
        {
            var result = _reviewService.GetReview(id);
            return Ok(result.ToDto());
        }

        // This is an endpoint exposed for searching reviews
        [HttpPost("search")]
        public IActionResult SearchReviews([FromBody] SearchRequestDto<Review> request)
        {
            var result = _reviewService.SearchReviews(request.ToPageRequest());
            var resultDtos = result.Data.Select(item => item.ToDto());
            return Ok(new Page<ReviewDto>
            {
                Data = resultDtos,
                Meta = result.Meta,
            });
        }

        // This is an endpoint exposed for deleting a review
        [HttpDelete("{id}")]
        public IActionResult DeleteReview([FromRoute] string id)
        {
            _reviewService.DeleteReview(id);
            return NoContent();
        }

        // This is an endpoint exposed for updating a review
        [HttpPut("{id}")]
        public IActionResult UpdateReview([FromRoute] string id, [FromBody] UpdateReviewRequestDto request)
        {
            var result = _reviewService.UpdateReview(id, request);
            return Ok(result.ToDto());
        }

        // This is an endpoint exposed for creating a review
        [HttpPost]
        public IActionResult CreateReview([FromBody] CreateReviewRequestDto request)
        {
            var result = _reviewService.CreateReview(request);
            return Ok(result.ToDto());
        }
    }
}