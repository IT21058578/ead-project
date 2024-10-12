
using api.DTOs.Requests;
using api.DTOs.Responses;
using api.Models;
using api.Utilities;
using Microsoft.AspNetCore.Identity;
using api.Repositories;
using api.DTOs.Templates;
using api.Configuratons;
using Microsoft.Extensions.Options;
using api.Exceptions;
using MongoDB.Bson;

namespace api.Services
{
    /// <summary>
    /// The AuthService class provides authentication and authorization services for the application.
    /// </summary>
    /// 
    /// <remarks>
    /// The AuthService class is responsible for handling user registration, login, password reset, and other authentication-related operations.
    /// It interacts with repositories and services to perform the necessary operations.
    /// </remarks>
    public class AuthService(
        JwtTokenService jwtTokenService,
        ILogger<NotificationService> logger,
        EmailService emailService,
        TokenService tokenService,
        UserRepository userRepository,
        CredentialRepository credentialRepository,
        PasswordHasher<User> passwordHasher,
        IOptions<ClientSettings> config)
    {
        private readonly ClientSettings _clientSettings = config.Value;
        private readonly ILogger<NotificationService> _logger = logger;
        private readonly UserRepository _userRepository = userRepository;
        private readonly CredentialRepository _credentialRepository = credentialRepository;
        private readonly PasswordHasher<User> _passwordHasher = passwordHasher;
        private readonly EmailService _emailService = emailService;
        private readonly TokenService _tokenService = tokenService;
        private readonly JwtTokenService _jwtTokenService = jwtTokenService;

        // This method is used to register a user
        public async Task Register(CreateUserRequestDto createUserRequestDto)
        {
            _logger.LogInformation("Registering user with email {Email}", createUserRequestDto.Email);
            var existingUser = await _userRepository.FindByEmailAsync(createUserRequestDto.Email);
            if (existingUser != null)
            {
                throw new ConflictException($"User with email {createUserRequestDto.Email} already exists");
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
                    VerificationLink = _clientSettings.WebApplicationUri + "/api/v1/register/auth/verify?code=" + token.Code + "&email=" + user.Email,
                    VerificationCode = token.Code
                }
            });
            _logger.LogInformation("User with email {Email} has been registered", createUserRequestDto.Email);
        }

        // This method is used to register a verified vendor user
        public async Task RegisterVendorUser(CreateUserRequestDto createUserRequestDto)
        {
            _logger.LogInformation("Registering user with email {Email}", createUserRequestDto.Email);
            var existingUser = await _userRepository.FindByEmailAsync(createUserRequestDto.Email);
            if (existingUser != null)
            {
                throw new ConflictException($"User with email {createUserRequestDto.Email} already exists");
            }
            var user = new User
            {
                Email = createUserRequestDto.Email,
                FirstName = createUserRequestDto.FirstName,
                LastName = createUserRequestDto.LastName,
                IsApproved = true,
                IsVerified = true,
                Rating = 0.0,
                Role = AppUserRole.Vendor,
            };
            var savedUser = await _userRepository.AddAsync(user);
            var credential = new Credential
            {
                UserId = savedUser.Id,
                Password = _passwordHasher.HashPassword(savedUser, createUserRequestDto.Password)
            };
            await _credentialRepository.AddAsync(credential);
            var token = _tokenService.CreateToken(TokenPurpose.Registration, user.Email);
            _logger.LogInformation("User with email {Email} has been registered", createUserRequestDto.Email);
        }


        // This method is used to resend the registration email
        public async Task ResendRegisterEmail(string email)
        {
            _logger.LogInformation("Resending register email to {Email}", email);
            var user = await GetUserOrThrow(email);
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
                    VerificationLink = _clientSettings.WebApplicationUri + "/api/v1/register/auth/verify?code=" + token.Code + "&email=" + user.Email,
                    VerificationCode = token.Code
                }
            });
            _logger.LogInformation("Register email has been resent to {Email}", email);
        }

        // This method is used to verify the registration
        public async Task VerifyRegistration(string code, string email)
        {
            _logger.LogInformation("Verifying registration with code {Code} and email {Email}", code, email);
            var token = _tokenService.ClaimToken(code, email);
            var user = await GetUserOrThrow(email);
            user.IsVerified = true;
            await _userRepository.UpdateAsync(user);
            _logger.LogInformation("Registration with code {Code} and email {Email} has been verified", code, email);
        }

        // This method is used to approve the registration
        public async Task ApproveRegistration(string email)
        {
            _logger.LogInformation("Approving registration for {Email}", email);
            var user = await GetUserOrThrow(email);
            user.IsApproved = true;
            await _userRepository.UpdateAsync(user);
            _logger.LogInformation("Registration for {Email} has been approved", email);
        }

        // This method is used to login a user
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            _logger.LogInformation("Logging in user with email {Email}", loginRequestDto.Email);
            var user = await GetUserOrThrow(loginRequestDto.Email);
            if (user.IsVerified == false)
            {
                throw new UnauthorizedException("User is not verified");
            }
            if (user.IsApproved == false)
            {
                throw new UnauthorizedException("User is not approved");
            }

            var credential = await _credentialRepository.FindByUserIdAsync(user.Id) ?? throw new NotFoundException($"Credential for user with email {loginRequestDto.Email} does not exist");
            var result = _passwordHasher.VerifyHashedPassword(user, credential.Password, loginRequestDto.Password);
            if (result.Equals(PasswordVerificationResult.Failed))
            {
                throw new UnauthorizedException("Password is incorrect");
            }
            _logger.LogInformation("User with email {Email} has been logged in", loginRequestDto.Email);
            return new()
            {
                Id = user.Id.ToString(),
                CreatedBy = user.CreatedBy.ToString(),
                CreatedAt = user.CreatedAt,
                UpdatedBy = user.UpdatedBy.ToString(),
                UpdatedAt = user.UpdatedAt,
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

        // This method is used to refresh the login
        public async Task<LoginResponseDto> RefreshLogin(string token)
        {
            _logger.LogInformation("Refreshing login with token {Token}...", token.Take(10));
            var principal = _jwtTokenService.ValidateRefreshToken(token);
            if (principal is null)
            {
                _logger.LogWarning("Invalid token with identity {Identity}", principal);
                throw new UnauthorizedException("Invalid token");
            }
            var email = _jwtTokenService.GetEmailFromToken(principal);
            if (email is null)
            {
                _logger.LogWarning("Invalid token with identity email {Email}", email);
                throw new UnauthorizedException("Invalid token");
            }
            var user = await GetUserOrThrow(email);
            _logger.LogInformation("User with email {Email} has been refreshed", email);
            return new()
            {
                Id = user.Id.ToString(),
                CreatedBy = user.CreatedBy.ToString(),
                CreatedAt = user.CreatedAt,
                UpdatedBy = user.UpdatedBy.ToString(),
                UpdatedAt = user.UpdatedAt,
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

        // This method is used to send a forgot password email
        public async Task ForgotPassword(string email)
        {
            _logger.LogInformation("Forgot password for {Email}", email);
            var user = await GetUserOrThrow(email);
            var token = _tokenService.CreateToken(TokenPurpose.ResetPasssword, user.Email);
            await _emailService.SendEmail(new EmailRequest<ResetPasswordTemplateData>
            {
                Subject = "Forgot Password",
                To = user.Email,
                TemplateName = EmailTemplate.ResetPassword,
                TemplateData = new ResetPasswordTemplateData
                {
                    FirstName = user.FirstName,
                    ResetPasswordLink = _clientSettings.WebApplicationUri + "/api/v1/auth/password/reset?code=" + token.Code + "&email=" + user.Email,
                    ResetPasswordCode = token.Code
                }
            });
            _logger.LogInformation("Forgot password email has been sent to {Email}", email);
        }

        // This method is used to reset the password
        public async Task ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            _logger.LogInformation("Resetting password for {Email}", resetPasswordRequestDto.Email);
            var token = _tokenService.ClaimToken(resetPasswordRequestDto.Code, resetPasswordRequestDto.Email);
            var user = await GetUserOrThrow(token.Email);
            var credential = await GetCredentialOrThrow(user.Id);
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

        // This method is used to change the password
        public async Task ChangePassword(ChangePasswordRequestDto changePasswordRequestDto)
        {
            _logger.LogInformation("Changing password for {Email}", changePasswordRequestDto.Email);
            var user = await GetUserOrThrow(changePasswordRequestDto.Email);
            var credential = await GetCredentialOrThrow(user.Id);
            if (!_passwordHasher.VerifyHashedPassword(user, credential.Password, changePasswordRequestDto.OldPassword).Equals(PasswordVerificationResult.Success))
            {
                throw new UnauthorizedException("Old password is incorrect");
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

        // This method is used to get a user or throw an exception
        private async Task<User> GetUserOrThrow(string email)
        {
            return await _userRepository.FindByEmailAsync(email) ?? throw new NotFoundException($"User with email {email} does not exist");
        }

        // This method is used to get a credential or throw an exception
        private async Task<Credential> GetCredentialOrThrow(ObjectId userId)
        {
            return await _credentialRepository.FindByUserIdAsync(userId) ?? throw new NotFoundException($"Credential for user with id {userId} does not exist");
        }
    }
}