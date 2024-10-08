using api.DTOs.Requests;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.V1
{
    /// <summary>
    /// The CustomerRequestController class represents the controller for handling customer request related operations.
    /// </summary>
    /// 
    /// <remarks>
    /// The CustomerRequestController class is responsible for handling customer request related operations.
    /// It contains an endpoint for creating an order cancellation request.
    /// </remarks>
    /// /// {
    [Route("api/v1/customer-requests")]
    [ApiController]
    public class CustomerRequestController(NotificationService notificationService) : ControllerBase
    {
        private readonly NotificationService _notificationService = notificationService;

        // This is an endpoint exposed for creating an order cancellation request
        [HttpPost("order-cancellation")]
        public void CancelOrder([FromBody] CreateOrderCancellationRequestDto request)
        {
            _notificationService.CreateNotification(request);
        }
    }
}