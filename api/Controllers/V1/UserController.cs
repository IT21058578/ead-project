using api.DTOs.Models;
using api.DTOs.Requests;
using api.Models;
using api.Services;
using api.Transformers;
using api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.V1
{
    /// <summary>
    /// The UserController class represents the controller for managing user-related operations.
    /// </summary>
    /// 
    /// <remarks>
    /// The UserController class is responsible for handling HTTP requests related to users.
    /// It provides endpoints for getting a user, searching users, and updating a user.
    /// </remarks>
    [Route("api/v1/users")]
    [ApiController]
    public class UserController(ILogger<UserController> logger, UserService userService) : ControllerBase
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly UserService _userService = userService;

        // This is an endpoint exposed for getting a user
        [HttpGet("{id}")]
        public IActionResult GetUser([FromRoute] string id)
        {
            var result = _userService.GetUser(id);
            return Ok(result.ToDto());
        }

        // This is an endpoint exposed for deleting a user
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

        // This is an endpoint exposed for deleting a user
        [HttpPut("{id}")]
        public IActionResult UpdateUser([FromRoute] string id, [FromBody] UpdateUserRequestDto request)
        {
            var result = _userService.UpdateUser(id, request);
            return Ok(result.ToDto());
        }
    }
}