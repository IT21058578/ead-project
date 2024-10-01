using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Requests;
using api.DTOs.Responses;
using api.Models;
using api.Utilities;
using Microsoft.AspNetCore.Identity;
using api.Repositories;
using api.DTOs.Templates;

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
            _logger.LogInformation("Registering user with email {Email}", createUserRequestDto.Email);
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
            await _emailService.SendEmail(new EmailRequest<RegisterTemplateData>
            {
                Subject = "Register",
                To = user.Email,
                TemplateName = EmailTemplate.Registration,
                TemplateData = new RegisterTemplateData
                {
                    FirstName = user.FirstName,
                    VerificationLink = "http://localhost:5000/api/v1/register/auth/verify?code=" + token.Code + "&email=" + user.Email,
                    VerificationCode = token.Code
                }
            });
            _logger.LogInformation("User with email {Email} has been registered", createUserRequestDto.Email);
        }

        public async Task ResendRegisterEmail(string email)
        {
            _logger.LogInformation("Resending register email to {Email}", email);
            var user = _userRepository.FindByEmail(email) ?? throw new Exception($"User with email {email} does not exist");
            _tokenService.ClaimAllToken(TokenPurpose.Registration, user.Email);
            var token = _tokenService.CreateToken(TokenPurpose.Registration, user.Email);
            await _emailService.SendEmail(new EmailRequest<RegisterTemplateData>
            {
                Subject = "Register",
                To = user.Email,
                TemplateName = EmailTemplate.Registration,
                TemplateData = new RegisterTemplateData
                {
                    FirstName = user.FirstName,
                    VerificationLink = "http://localhost:5000/api/v1/register/auth/verify?code=" + token.Code + "&email=" + user.Email,
                    VerificationCode = token.Code
                }
            });
            _logger.LogInformation("Register email has been resent to {Email}", email);
        }

        public async Task VerifyRegistration(string code, string email)
        {
            _logger.LogInformation("Verifying registration with code {Code} and email {Email}", code, email);
            var token = _tokenService.ClaimToken(code, email);
            var user = _userRepository.FindByEmail(token.Email) ?? throw new Exception($"User with email {email} does not exist");
            user.IsVerified = true;
            await _userRepository.UpdateAsync(user);
            _logger.LogInformation("Registration with code {Code} and email {Email} has been verified", code, email);
        }

        public async Task ApproveRegistration(string email)
        {
            _logger.LogInformation("Approving registration for {Email}", email);
            var user = _userRepository.FindByEmail(email) ?? throw new Exception($"User with email {email} does not exist");
            user.IsApproved = true;
            await _userRepository.UpdateAsync(user);
            _logger.LogInformation("Registration for {Email} has been approved", email);
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            _logger.LogInformation("Logging in user with email {Email}", loginRequestDto.Email);
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
            _logger.LogInformation("User with email {Email} has been logged in", loginRequestDto.Email);
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
                RefreshToken = _jwtTokenService.CreateRefreshToken(user)
            };
        }

        public async Task<LoginResponseDto> RefreshLogin(string token)
        {
            _logger.LogInformation("Refreshing login with token {Token}...", token.Take(10));
            var principal = _jwtTokenService.ValidateRefreshToken(token);
            if (principal is null)
            {
                _logger.LogWarning("Invalid token with identity {Identity}", principal);
                throw new Exception("Invalid token");
            }
            var email = _jwtTokenService.GetEmailFromToken(principal);
            if (email is null)
            {
                _logger.LogWarning("Invalid token with identity email {Email}", email);
                throw new Exception("Invalid token");
            }
            var user = await _userRepository.FindByEmailAsync(email) ?? throw new Exception("User not found");
            _logger.LogInformation("User with email {Email} has been refreshed", email);
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
                RefreshToken = _jwtTokenService.CreateRefreshToken(user)
            };
        }

        public async Task ForgotPassword(string email)
        {
            _logger.LogInformation("Forgot password for {Email}", email);
            var user = _userRepository.FindByEmail(email) ?? throw new Exception($"User with email {email} does not exist");
            var token = _tokenService.CreateToken(TokenPurpose.ResetPasssword, user.Email);
            await _emailService.SendEmail(new EmailRequest<ResetPasswordTemplateData>
            {
                Subject = "Forgot Password",
                To = user.Email,
                TemplateName = EmailTemplate.ResetPassword,
                TemplateData = new ResetPasswordTemplateData
                {
                    FirstName = user.FirstName,
                    ResetPasswordLink = "http://localhost:5000/api/v1/auth/password/reset?code=" + token.Code + "&email=" + user.Email,
                    ResetPasswordCode = token.Code
                }
            });
            _logger.LogInformation("Forgot password email has been sent to {Email}", email);
        }

        public async Task ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            _logger.LogInformation("Resetting password for {Email}", resetPasswordRequestDto.Email);
            var token = _tokenService.ClaimToken(resetPasswordRequestDto.Code, resetPasswordRequestDto.Email);
            var user = _userRepository.FindByEmail(token.Email) ?? throw new Exception($"User with email {resetPasswordRequestDto.Email} does not exist");
            var credential = _credentialRepository.FindByUserId(user.Id) ?? throw new Exception($"Credential for user with email {resetPasswordRequestDto.Email} does not exist");
            credential.Password = _passwordHasher.HashPassword(user, resetPasswordRequestDto.NewPassword);
            await _credentialRepository.UpdateAsync(credential);
            await _emailService.SendEmail(new EmailRequest<PasswordChangedTemplateData>
            {
                Subject = "Password Changed",
                To = user.Email,
                TemplateName = EmailTemplate.PasswordChanged,
                TemplateData = new PasswordChangedTemplateData
                {
                    FirstName = user.FirstName
                }
            });
            _logger.LogInformation("Password for {Email} has been reset", resetPasswordRequestDto.Email);
        }

        public async Task ChangePassword(ChangePasswordRequestDto changePasswordRequestDto)
        {
            _logger.LogInformation("Changing password for {Email}", changePasswordRequestDto.Email);
            var user = _userRepository.FindByEmail(changePasswordRequestDto.Email) ?? throw new Exception($"User with email {changePasswordRequestDto.Email} does not exist");
            var credential = _credentialRepository.FindByUserId(user.Id) ?? throw new Exception($"Credential for user with email {changePasswordRequestDto.Email} does not exist");
            if (!_passwordHasher.VerifyHashedPassword(user, credential.Password, changePasswordRequestDto.OldPassword).Equals(PasswordVerificationResult.Success))
            {
                throw new Exception("Old password is incorrect");
            }
            credential.Password = _passwordHasher.HashPassword(user, changePasswordRequestDto.NewPassword);
            await _credentialRepository.UpdateAsync(credential);
            await _emailService.SendEmail(new EmailRequest<PasswordChangedTemplateData>
            {
                Subject = "Password Changed",
                To = user.Email,
                TemplateName = EmailTemplate.PasswordChanged,
                TemplateData = new PasswordChangedTemplateData
                {
                    FirstName = user.FirstName
                }
            });
            _logger.LogInformation("Password for {Email} has been changed", changePasswordRequestDto.Email);
        }
    }
}