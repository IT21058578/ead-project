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

namespace api.V1.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController(ILogger<OrderController> logger, OrderService orderService) : ControllerBase
    {
        private readonly ILogger<OrderController> _logger = logger;
        private readonly OrderService _orderService = orderService;


        [HttpGet("{id}")]
        public IActionResult GetOrder([FromRoute] String id)
        {
            var result = _orderService.GetOrder(id);
            return Ok(result);
        }

        [HttpPost("search")]
        public IActionResult SearchOrders([FromBody] SearchRequestDto<Order> request)
        {
            var result = _orderService.SearchOrders();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder([FromRoute] String id)
        {
            _orderService.DeleteOrder(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder([FromRoute] String id, [FromBody] CreateOrderRequestDto request)
        {
            var result = _orderService.UpdateOrder(id, request);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderRequestDto request)
        {
            var result = _orderService.CreateOrder(request);
            return Ok(result);
        }

    }
}