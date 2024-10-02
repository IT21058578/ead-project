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
            return Ok(result.ToDto());
        }

        [HttpPost("search")]
        public IActionResult SearchUsers([FromBody] SearchRequestDto<User> request)
        {
            var result = _userService.SearchUsers(request.ToPageRequest());
            var resultDtos = result.Data.Select(item => item.ToDto());
            return Ok(new Page<UserDto>
            {
                Data = resultDtos,
                Meta = result.Meta,
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser([FromRoute] string id, [FromBody] UpdateUserRequestDto request)
        {
            var result = _userService.UpdateUser(id, request);
            return Ok(result.ToDto());
        }
    }
}