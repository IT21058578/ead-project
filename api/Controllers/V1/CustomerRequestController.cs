using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.V1
{
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