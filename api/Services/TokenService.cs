
using api.Exceptions;
using api.Models;
using api.Repositories;
using api.Utilities;

namespace api.Services
{
    /// <summary>
    /// The TokenService class provides methods for creating, claiming, and validating tokens.
    /// </summary>
    /// 
    /// <remarks>
    /// The TokenService class is responsible for managing tokens in the system.
    /// It contains methods for creating a token, claiming a token, claiming all tokens,
    /// and validating a token. The class relies on a TokenRepository for data storage
    /// and a logger for logging purposes.
    /// </remarks>
    public class TokenService(TokenRepository tokenRepository, ILogger<TokenService> logger)
    {
        private readonly ILogger<TokenService> _logger = logger;
        private readonly TokenRepository _tokenRepository = tokenRepository;

        // This is a method for creating a token
        public Token CreateToken(TokenPurpose purpose, string email)
        {
            _logger.LogInformation("Creating token with purpose {Purpose} and email {Email}", purpose, email);
            var result = _tokenRepository.Add(new()
            {
                Code = "12345",
                Email = email,
                Purpose = purpose,
                Status = TokenStatus.Active
            });
            _logger.LogInformation("Token with {Id} has been created", result.Id);
            return result;
        }

        // This is a method for claiming a token
        public Token ClaimToken(string code, string email)
        {
            _logger.LogInformation("Claiming token with code {Code} and email {Email}", code, email);
            var result = _tokenRepository.FindByCodeAndEmail(code, email) ?? throw new NotFoundException($"Token with code {code} and email {email} not found");
            if (!IsTokenValid(result))
            {
                _logger.LogWarning("Cannot claim token with {Id} as it has status {Status}", result.Id, result.Status);
                throw new BadRequestException("Token is not valid");
            }
            result.Status = TokenStatus.Claimed;
            _tokenRepository.Update(result);
            _logger.LogInformation("Token with {Id} has been claimed", result.Id);
            return result;
        }

        // This is a method for claiming all tokens
        public void ClaimAllToken(TokenPurpose purpose, string email)
        {
            _logger.LogInformation("Claiming all tokens with purpose {Purpose} and email {Email}", purpose, email);
            var tokens = _tokenRepository.FindByPurposeAndEmailAndStatusIsNotClaimed(purpose, email);
            foreach (var token in tokens)
            {
                token.Status = TokenStatus.Claimed;
            }
            _tokenRepository.UpdateMany(tokens);
            _logger.LogInformation("All tokens with purpose {Purpose} and email {Email} have been claimed", purpose, email);
        }

        // This is a method for validating a token
        public bool IsTokenValid(Token token)
        {
            _logger.LogInformation("Checking if token with {Id} is valid", token.Id);
            return token.Status == TokenStatus.Active;
        }
    }
}