

namespace api.DTOs.Models
{
    /// <summary>
    /// The ReviewDto class represents the data transfer object for a review.
    /// </summary>
    /// 
    /// <remarks>
    /// The ReviewDto class is used to transfer data for a review.
    /// It contains properties for the ID, creator, creation date, updater, update date,
    /// vendor ID, product ID, user ID, message, and rating of the review.
    /// </remarks>
    public class ReviewDto
    {
        public string Id { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
        public string VendorId { get; set; } = "";
        public string ProductId { get; set; } = "";
        public string UserId { get; set; } = "";
        public string Message { get; set; } = "";
        public double Rating { get; set; } = 0;
    }
}