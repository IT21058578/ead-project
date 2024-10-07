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
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController(ILogger<OrderController> logger, OrderService orderService) : ControllerBase
    {
        private readonly ILogger<OrderController> _logger = logger;
        private readonly OrderService _orderService = orderService;

        // This is an endpoint exposed for getting an order
        [HttpGet("{id}")]
        public IActionResult GetOrder([FromRoute] string id)
        {
            var result = _orderService.GetOrder(id);
            return Ok(result.ToDto());
        }

        // This is an endpoint exposed for searching orders
        [HttpPost("search")]
        public IActionResult SearchOrders([FromBody] SearchRequestDto<Order> request)
        {
            var result = _orderService.SearchOrders(request.ToPageRequest());
            var resultDtos = result.Data.Select(item => item.ToDto());
            return Ok(new Page<OrderDto>
            {
                Data = resultDtos,
                Meta = result.Meta,
            });
        }

        // This is an endpoint exposed for deleting an order
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder([FromRoute] string id)
        {
            _orderService.DeleteOrder(id);
            return NoContent();
        }

        // This is an endpoint exposed for updating an order
        [HttpPut("{id}")]
        public IActionResult UpdateOrder([FromRoute] string id, [FromBody] UpdateOrderRequestDto request)
        {
            var result = _orderService.UpdateOrder(id, request);
            return Ok(result.ToDto());
        }

        // This is an endpoint exposed for creating an order
        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderRequestDto request)
        {
            var result = _orderService.CreateOrder(request);
            return Ok(result.ToDto());
        }
    }
}