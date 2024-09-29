using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.DTOs.Responses;
using api.Models;
using api.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace api.Services
{
    public class AuthService(UserManager<User> userManager, ILogger<NotificationService> logger, EmailService emailService)
    {
        private readonly ILogger<NotificationService> _logger = logger;
        private readonly UserManager<User> _userManager = userManager;
        private readonly EmailService _emailService = emailService;

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
            await _emailService.SendEmail(new EmailRequest
            {
                Subject = "Register",
                To = user.Email,
                TemplateName = EmailRequest.Template.Registration,
                TemplateData = {
                    {"Code", "123456"}
                }
            });
        }

        public async Task ResendRegisterEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception($"User with email {email} does not exist");
            await _emailService.SendEmail(new EmailRequest
            {
                Subject = "Register",
                To = user.Email,
                TemplateName = EmailRequest.Template.Registration,
                TemplateData = {
                    {"Code", "123456"}
                }
            });
        }

        public async Task VerifyRegistration(string code)
        {
            
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            return new();
        }

        public async Task<LoginResponseDto> RefreshLogin(string authorization)
        {
            return new();
        }

        public async Task ForgotPassword(string email)
        {
        }

        public async Task ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
        }

        public async Task ChangePassword(ChangePasswordRequestDto changePasswordRequestDto)
        {
        }
    }
}