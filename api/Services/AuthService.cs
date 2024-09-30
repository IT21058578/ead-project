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
using api.Repositories;

namespace api.Services
{
    public class AuthService(
        JwtTokenService jwtTokenService,
        ILogger<NotificationService> logger,
        EmailService emailService,
        TokenService tokenService,
        UserRepository userRepository,
        CredentialRepository credentialRepository,
        PasswordHasher<User> passwordHasher)
    {
        private readonly ILogger<NotificationService> _logger = logger;
        private readonly UserRepository _userRepository = userRepository;
        private readonly CredentialRepository _credentialRepository = credentialRepository;
        private readonly PasswordHasher<User> _passwordHasher = passwordHasher;
        private readonly EmailService _emailService = emailService;
        private readonly TokenService _tokenService = tokenService;
        private readonly JwtTokenService _jwtTokenService = jwtTokenService;

        public async Task Register(CreateUserRequestDto createUserRequestDto)
        {
            var existingUser = _userRepository.FindByEmail(createUserRequestDto.Email);
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
            var savedUser = await _userRepository.AddAsync(user);
            var credential = new Credential
            {
                UserId = savedUser.Id,
                Password = _passwordHasher.HashPassword(savedUser, createUserRequestDto.Password)
            };
            await _credentialRepository.AddAsync(credential);
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
            var user = _userRepository.FindByEmail(email) ?? throw new Exception($"User with email {email} does not exist");
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
            var user = _userRepository.FindByEmail(token.Email) ?? throw new Exception($"User with email {email} does not exist");
            user.IsVerified = true;
            await _userRepository.UpdateAsync(user);
        }

        public async Task ApproveRegistration(string email)
        {
            var user = _userRepository.FindByEmail(email) ?? throw new Exception($"User with email {email} does not exist");
            user.IsApproved = true;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _userRepository.FindByEmail(loginRequestDto.Email) ?? throw new Exception($"User with email {loginRequestDto.Email} does not exist");
            if (user.IsVerified == false)
            {
                throw new Exception("User is not verified");
            }
            if (user.IsApproved == false)
            {
                throw new Exception("User is not approved");
            }

            var credential = _credentialRepository.FindByUserId(user.Id) ?? throw new Exception($"Credential for user with email {loginRequestDto.Email} does not exist");
            var result = _passwordHasher.VerifyHashedPassword(user, credential.Password, loginRequestDto.Password);
            if (result.Equals(PasswordVerificationResult.Failed))
            {
                throw new Exception("Password is incorrect");
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
            var user = await _userRepository.FindByEmailAsync(principal.Identity.Name) ?? throw new Exception("User not found");
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
            var user = _userRepository.FindByEmail(email) ?? throw new Exception($"User with email {email} does not exist");
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
            var user = _userRepository.FindByEmail(token.Email) ?? throw new Exception($"User with email {resetPasswordRequestDto.Email} does not exist");
            var credential = _credentialRepository.FindByUserId(user.Id) ?? throw new Exception($"Credential for user with email {resetPasswordRequestDto.Email} does not exist");
            credential.Password = _passwordHasher.HashPassword(user, resetPasswordRequestDto.NewPassword);
            await _credentialRepository.UpdateAsync(credential);
        }

        public async Task ChangePassword(ChangePasswordRequestDto changePasswordRequestDto)
        {
            var user = _userRepository.FindByEmail(changePasswordRequestDto.Email) ?? throw new Exception($"User with email {changePasswordRequestDto.Email} does not exist");
            var credential = _credentialRepository.FindByUserId(user.Id) ?? throw new Exception($"Credential for user with email {changePasswordRequestDto.Email} does not exist");
            if (!_passwordHasher.VerifyHashedPassword(user, credential.Password, changePasswordRequestDto.OldPassword).Equals(PasswordVerificationResult.Success))
            {
                throw new Exception("Old password is incorrect");
            }
            credential.Password = _passwordHasher.HashPassword(user, changePasswordRequestDto.NewPassword);
            await _credentialRepository.UpdateAsync(credential);
        }
    }
}