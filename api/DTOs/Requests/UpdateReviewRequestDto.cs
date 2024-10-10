
using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The UpdateReviewRequestDto class represents the data transfer object for updating a review.
    /// </summary>
    /// 
    /// <remarks>
    /// The UpdateReviewRequestDto class is used to transfer data for updating a review.
    /// It contains properties for the review message and rating.
    /// The message property is required and must have a minimum length of 1 and a maximum length of 500 characters.
    /// The rating property is required and must be a value between 0 and 5.
    /// </remarks>
    public class UpdateReviewRequestDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Message { get; set; } = "";
        [Required]
        [Range(0, 5)]
        public double Rating { get; set; } = 0;
    }
}