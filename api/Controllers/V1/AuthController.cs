using api.DTOs.Requests;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.V1
{
    /// <summary>
    /// The AuthController class represents the controller for handling authentication-related endpoints.
    /// </summary>
    /// 
    /// <remarks>
    /// The AuthController class is responsible for handling user registration, email verification, login, password management, and other authentication-related operations.
    /// It contains endpoints for user registration, email re-sending, verification, approval, login, refresh login, logout, forgot password, password reset, and password change.
    /// </remarks>
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController(ILogger<AuthController> logger, AuthService authService) : ControllerBase
    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly AuthService _authService = authService;

        // This is an endpoint exposed for user registration
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequestDto createUserRequestDto)
        {
            await _authService.Register(createUserRequestDto);
            return Ok();
        }

        // This is an endpoint exposed for vendor registration
        [HttpPost("register/vendor")] 
        public async Task<IActionResult> RegisterVendorUser([FromBody] CreateUserRequestDto createUserRequestDto)
        {
            await _authService.RegisterVendorUser(createUserRequestDto);
            return Ok();
        }

        // This is an endpoint exposed for user registration email re-sending
        [HttpPost("register/re-send")]
        public async Task<IActionResult> ResendRegisterEmail([FromQuery] string email)
        {
            await _authService.ResendRegisterEmail(email);
            return Ok();
        }

        // This is an endpoint exposed for user registration verification
        [HttpPut("register/verify")]
        public async Task<IActionResult> VerifyRegistration([FromQuery] string code, [FromQuery] string email)
        {
            await _authService.VerifyRegistration(code, email);
            return Ok();
        }

        // This is an endpoint exposed for user registration approval
        [HttpPut("register/approve")]
        public async Task<IActionResult> ApproveRegistration([FromQuery] string email)
        {
            await _authService.ApproveRegistration(email);
            return Ok();
        }

        // This is an endpoint exposed for user login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var result = await _authService.Login(loginRequestDto);
            return Ok(result);
        }

        // This is an endpoint exposed for user login refresh
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshLogin([FromBody] RefreshLoginRequestDto refreshLoginRequestDto)
        {
            var result = await _authService.RefreshLogin(refreshLoginRequestDto.RefreshToken);
            return Ok(result);
        }

        // This is an endpoint exposed for user logout
        [HttpPut("password/forgot")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            await _authService.ForgotPassword(email);
            return Ok();
        }

        // This is an endpoint exposed for user password reset
        [HttpPut("password/reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
        {
            await _authService.ResetPassword(resetPasswordRequestDto);
            return Ok();
        }

        // This is an endpoint exposed for user password change
        [HttpPut("password/change")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePasswordRequestDto)
        {
            await _authService.ChangePassword(changePasswordRequestDto);
            return Ok();
        }
    }
}