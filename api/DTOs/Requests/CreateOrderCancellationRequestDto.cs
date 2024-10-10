

using System.ComponentModel.DataAnnotations;
using api.Annotations.Validation;

namespace api.DTOs.Requests
{
    /// <summary>
    /// The CreateOrderCancellationRequestDto class represents the data required for creating an order cancellation request.
    /// </summary>
    /// 
    /// <remarks>
    /// The CreateOrderCancellationRequestDto class is used to provide the necessary data for creating an order cancellation request.
    /// It contains the OrderId property which represents the ID of the order to be cancelled.
    /// It contains the UserId property which represents the ID of the user requesting the cancellation.
    /// It contains the Reason property which represents the reason for the cancellation.
    /// </remarks>
    public class CreateOrderCancellationRequestDto
    {
        [Required]
        [ValidObjectId]
        public string OrderId { get; set; } = "";
        [Required]
        [ValidObjectId]
        public string UserId { get; set; } = "";
        [Required]
        public string Reason { get; set; } = "";
    }
}