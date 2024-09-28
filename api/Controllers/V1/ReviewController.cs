using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Models;
using api.Services;
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

        [HttpGet("{id}")]
        public IActionResult GetReview([FromRoute] string id)
        {
            var result = _reviewService.GetReview(id);
            return Ok(result);
        }

        [HttpPost("search")]
        public IActionResult SearchReviews([FromBody] SearchRequestDto<Review> request)
        {
            var result = _reviewService.SearchReviews(request.ToPageRequest());
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReview([FromRoute] string id)
        {
            _reviewService.DeleteReview(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReview([FromRoute] string id, [FromBody] CreateReviewRequestDto request)
        {
            var result = _reviewService.UpdateReview(id, request);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateReview([FromBody] CreateReviewRequestDto request)
        {
            var result = _reviewService.CreateReview(request);
            return Ok(result);
        }
    }
}