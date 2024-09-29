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
            var result = _tokenRepository.FindByCodeAndEmail(code, email) ?? throw new Exception("Token does not exist");
            if (!IsTokenValid(result))
            {
                throw new Exception("Token is not valid");
            }
            result.Status = TokenStatus.Claimed;
            _tokenRepository.Update(result);
            return result;
        }

        public bool IsTokenValid(Token token)
        {
            return new List<TokenStatus> { TokenStatus.Claimed, TokenStatus.Expired, TokenStatus.Revoked }.Contains(token.Status);
        }
    }
}