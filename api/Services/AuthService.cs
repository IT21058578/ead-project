using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.DTOs.Responses;
using api.Models;
using api.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;

namespace api.Services
{
    public class AuthService(
        JwtTokenService jwtTokenService,
        UserManager<User> userManager,
        ILogger<NotificationService> logger,
        EmailService emailService,
        TokenService tokenService,
        SignInManager<User> signInManager)
    {
        private readonly ILogger<NotificationService> _logger = logger;
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly EmailService _emailService = emailService;
        private readonly TokenService _tokenService = tokenService;
        private readonly JwtTokenService _jwtTokenService = jwtTokenService;

        public async Task Register(CreateUserRequestDto createUserRequestDto)
        {
            var existingUser = _userManager.Users.FirstOrDefault(x => x.Email == createUserRequestDto.Email);
            if (existingUser != null)
            {
                throw new Exception($"User with email {createUserRequestDto.Email} already exists");
            }
            var user = new User
            {
                Email = createUserRequestDto.Email,
                FirstName = createUserRequestDto.FirstName,
                LastName = createUserRequestDto.LastName,
                IsApproved = false,
                IsVerified = false,
                Rating = 0.0,
            };
            // TODO: Make sure this works and credentials get created
            var savedUser = await _userManager.CreateAsync(user, createUserRequestDto.Password);
            var token = _tokenService.CreateToken(TokenPurpose.Registration, user.Email);
            await _emailService.SendEmail(new EmailRequest
            {
                Subject = "Register",
                To = user.Email,
                TemplateName = EmailRequest.Template.Registration,
                TemplateData = {
                    {"Code", token.Code}
                }
            });
        }

        public async Task ResendRegisterEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception($"User with email {email} does not exist");
            _tokenService.ClaimAllToken(TokenPurpose.Registration, user.Email);
            var token = _tokenService.CreateToken(TokenPurpose.Registration, user.Email);
            await _emailService.SendEmail(new EmailRequest
            {
                Subject = "Register",
                To = user.Email,
                TemplateName = EmailRequest.Template.Registration,
                TemplateData = {
                    {"Code", token}
                }
            });
        }

        public async Task VerifyRegistration(string code, string email)
        {
            var token = _tokenService.ClaimToken(code, email);
            var user = await _userManager.FindByEmailAsync(token.Email) ?? throw new Exception($"User with email {email} does not exist");
            user.IsVerified = true;
            await _userManager.UpdateAsync(user);
        }

        public async Task ApproveRegistration(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception($"User with email {email} does not exist");
            user.IsApproved = true;
            await _userManager.UpdateAsync(user);
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email) ?? throw new Exception($"User with email {loginRequestDto.Email} does not exist");
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequestDto.Password, false);
            if (!result.Succeeded)
            {
                throw new Exception("Could not sign in");
            }
            return new()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsApproved = user.IsApproved,
                IsVerified = user.IsVerified,
                Rating = user.Rating,
                Role = user.Role,
                AccessToken = _jwtTokenService.CreateAccessToken(user),
                RefreshTokenn = _jwtTokenService.CreateRefreshToken(user)
            };
        }

        public async Task<LoginResponseDto> RefreshLogin(string token)
        {
            var principal = _jwtTokenService.ValidateRefreshToken(token);
            if (principal?.Identity?.Name is null)
            {
                throw new Exception("Invalid token");
            }
            var user = await _userManager.FindByNameAsync(principal.Identity.Name) ?? throw new Exception("User not found");
            return new()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsApproved = user.IsApproved,
                IsVerified = user.IsVerified,
                Rating = user.Rating,
                Role = user.Role,
                AccessToken = _jwtTokenService.CreateAccessToken(user),
                RefreshTokenn = _jwtTokenService.CreateRefreshToken(user)
            };
        }

        public async Task ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception($"User with email {email} does not exist");
            var token = _tokenService.CreateToken(TokenPurpose.ResetPasssword, user.Email);
            await _emailService.SendEmail(new EmailRequest
            {
                Subject = "Forgot Password",
                To = user.Email,
                TemplateName = EmailRequest.Template.ResetPassword,
                TemplateData = {
                    {"Code", token.Code}
                }
            });
        }

        public async Task ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var token = _tokenService.ClaimToken(resetPasswordRequestDto.Code, resetPasswordRequestDto.Email);
            var user = await _userManager.FindByEmailAsync(token.Email) ?? throw new Exception($"User with email {resetPasswordRequestDto.Email} does not exist");
            var result = await _userManager.ResetPasswordAsync(user, token.Code, resetPasswordRequestDto.NewPassword);
            if (!result.Succeeded)
            {
                throw new Exception("Could not reset password");
            }
        }

        public async Task ChangePassword(ChangePasswordRequestDto changePasswordRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(changePasswordRequestDto.Email) ?? throw new Exception($"User with email {changePasswordRequestDto.Email} does not exist");
            var result = await _userManager.ChangePasswordAsync(user, changePasswordRequestDto.OldPassword, changePasswordRequestDto.NewPassword);
            if (!result.Succeeded)
            {
                throw new Exception("Could not change password");
            }
        }
    }
}