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
    [Route("api/v1/users")]
    [ApiController]
    public class UserController(ILogger<UserController> logger, UserService userService) : ControllerBase
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly UserService _userService = userService;

        [HttpGet("{id}")]
        public IActionResult GetUser([FromRoute] string id)
        {
            var result = _userService.GetUser(id);
            return Ok(result);
        }

        [HttpPost("search")]
        public IActionResult SearchUsers([FromBody] SearchRequestDto<User> request)
        {
            var result = _userService.SearchUsers(request.ToPageRequest());
            return Ok(result);
        }
    }
}