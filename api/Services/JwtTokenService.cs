using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Configuratons;
using api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class JwtTokenService(IOptions<JwtSettings> config, ILogger<JwtTokenService> logger)
    {
        private readonly ILogger<JwtTokenService> _logger = logger;
        private readonly IOptions<JwtSettings> _config = config;
        private readonly SymmetricSecurityKey _accessKey = new(Encoding.UTF8.GetBytes(config.Value.AccessSecret));
        private readonly SymmetricSecurityKey _refreshKey = new(Encoding.UTF8.GetBytes(config.Value.RefreshSecret));
        public string CreateAccessToken(User user)
        {
            _logger.LogInformation("Creating access token for {Email}", user.Email);
            var claims = new List<Claim> {
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.GivenName, $"{user.FirstName} {user.LastName}"),
                new(JwtRegisteredClaimNames.NameId, ""),
            };
            var creds = new SigningCredentials(_accessKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(_config.Value.AccessExpiryMs),
                SigningCredentials = creds,
                Issuer = _config.Value.Issuer,
                Audience = _config.Value.Audience
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = tokenHandler.WriteToken(token);
            _logger.LogInformation("Access token for {Email} has been created", user.Email);
            return result;
        }

        public string CreateRefreshToken(User user)
        {
            _logger.LogInformation("Creating refresh token for {Email}", user.Email);
            var claims = new List<Claim> {
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.GivenName, $"{user.FirstName} {user.LastName}"),
                new(JwtRegisteredClaimNames.NameId, ""),
            };
            var creds = new SigningCredentials(_refreshKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now,
                SigningCredentials = creds,
                Issuer = _config.Value.Issuer,
                Audience = _config.Value.Audience,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = tokenHandler.WriteToken(token);
            _logger.LogInformation("Refresh token for {Email} has been created", user.Email);
            return result;
        }

        private ClaimsPrincipal? ValidateAccessToken(string token)
        {
            _logger.LogInformation("Validating access token {Token}", token.Take(10));
            var validation = new TokenValidationParameters
            {
                IssuerSigningKey = _accessKey,
                ValidateLifetime = false,
                ValidateActor = false,
                ValidateIssuer = false,
                ValidateAudience = false,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(token, validation, out _);
        }

        public ClaimsPrincipal? ValidateRefreshToken(string token)
        {
            _logger.LogInformation("Validating refresh token {Token}", token.Take(10));
            var validation = new TokenValidationParameters
            {
                IssuerSigningKey = _refreshKey,
                ValidateLifetime = false,
                ValidateActor = false,
                ValidateIssuer = false,
                ValidateAudience = false,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(token, validation, out _);
        }

        public string? GetEmailFromToken(ClaimsPrincipal principal)
        {
            _logger.LogInformation("Getting email from principal");
            return principal.FindFirstValue(ClaimTypes.Email);
        }
    }
}