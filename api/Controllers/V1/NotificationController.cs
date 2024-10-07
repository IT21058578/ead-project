using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Models;
using api.DTOs.Requests;
using api.Models;
using api.Services;
using api.Transformers;
using api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.V1
{
    [Route("api/v1/notifications")]
    [ApiController]
    public class NotificationController(NotificationService notificationService) : ControllerBase
    {
        private readonly NotificationService _notificationService = notificationService;

        // This is an endpoint exposed for deleting a notification
        [HttpDelete("{id}")]
        public IActionResult DeleteNotification([FromRoute] string id)
        {
            _notificationService.DeleteNotification(id);
            return NoContent();
        }

        // This is an endpoint exposed for creating a notification
        [HttpGet("{id}")]
        public IActionResult GetNotification([FromRoute] string id)
        {
            var result = _notificationService.GetNotification(id);
            return Ok(result.ToDto());
        }

        // This is an endpoint exposed for searching notifications
        [HttpPost("search")]
        public IActionResult SearchNotifications([FromBody] SearchRequestDto<Notification> request)
        {
            var result = _notificationService.SearchNotifications(request.ToPageRequest());
            var resultDtos = result.Data.Select(item => item.ToDto());
            return Ok(new Page<NotificationDto>
            {
                Data = resultDtos,
                Meta = result.Meta,
            });
        }

        // This is an endpoint exposed for updating a notification
        [HttpPut("{id}")]
        public IActionResult UpdateNotification([FromRoute] string id, [FromBody] UpdateNotificationRequestDto request)
        {
            var result = _notificationService.UpdateNotification(id, request);
            return Ok(result.ToDto());
        }

    }
}