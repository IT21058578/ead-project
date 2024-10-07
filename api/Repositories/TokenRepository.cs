using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Configurations;
using api.Models;
using api.Utilities;

namespace api.Repositories
{
    public class TokenRepository(AppDbContext dbContext) : MongoRepository<Token>(dbContext)
    {
        // A repository method for finding token by the code and email
        public Token? FindByCodeAndEmail(string code, string email)
        {
            return _dbSet.Where(o => o.Email == email).Where(o => o.Code == code).FirstOrDefault();
        }

        // A repository method for finding token by the purpose and email
        public IEnumerable<Token> FindByPurposeAndEmailAndStatusIsNotClaimed(TokenPurpose purpose, string email)
        {
            return _dbSet.Where(o => o.Purpose == purpose).Where(o => o.Email == email).ToList();
        }
    }
}