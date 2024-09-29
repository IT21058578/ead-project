using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.V1
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController(ILogger<AuthController> logger, AuthService authService) : ControllerBase
    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly AuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequestDto createUserRequestDto)
        {
            await _authService.Register(createUserRequestDto);
            return Ok();
        }

        [HttpPost("register/re-send")]
        public async Task<IActionResult> ResendRegisterEmail([FromQuery] String email)
        {
            await _authService.ResendRegisterEmail(email);
            return Ok();
        }

        [HttpPut("register/verify")]
        public async Task<IActionResult> VerifyRegistration([FromQuery] String code)
        {
            await _authService.VerifyRegistration(code);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var result = await _authService.Login(loginRequestDto);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshLogin([FromHeader] String authorization)
        {
            var result = await _authService.RefreshLogin(authorization);
            return Ok(result);
        }

        [HttpPut("password/forgot")]
        public async Task<IActionResult> ForgotPassword([FromQuery] String email)
        {
            await _authService.ForgotPassword(email);
            return Ok();
        }

        [HttpPut("password/reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
        {
            await _authService.ResetPassword(resetPasswordRequestDto);
            return Ok();
        }

        [HttpPut("password/change")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePasswordRequestDto)
        {
            await _authService.ChangePassword(changePasswordRequestDto);
            return Ok();
        }
    }
}