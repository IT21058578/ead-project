using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Repositories;
using api.Utilities;

namespace api.Services
{
    public class TokenService(TokenRepository tokenRepository, ILogger<TokenService> logger)
    {
        private readonly ILogger<TokenService> _logger = logger;
        private readonly TokenRepository _tokenRepository = tokenRepository;

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
            return result;
        }

        public Token ClaimToken(string code, string email)
        {
            _logger.LogInformation("Claiming token with code {Code} and email {Email}", code, email);
            var result = _tokenRepository.FindByCodeAndEmail(code, email) ?? throw new Exception("Token does not exist");
            if (!IsTokenValid(result))
            {
                _logger.LogWarning("Cannot claim token with {Id} as it has status {Status}", result.Id, result.Status);
                throw new Exception("Token is not valid");
            }
            result.Status = TokenStatus.Claimed;
            _tokenRepository.Update(result);
            _logger.LogInformation("Token with {Id} has been claimed", result.Id);
            return result;
        }

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

        public bool IsTokenValid(Token token)
        {
            _logger.LogInformation("Checking if token with {Id} is valid", token.Id);
            return token.Status == TokenStatus.Active;
        }
    }
}