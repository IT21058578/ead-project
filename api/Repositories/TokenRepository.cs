using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Configurations;
using api.Models;

namespace api.Repositories
{
    public class TokenRepository(AppDbContext dbContext) : MongoRepository<Token>(dbContext)
    {
        public Token? FindByCodeAndEmail(string code, string email)
        {
            return _dbSet.Where(o => o.Email == email).Where(o => o.Code == code).FirstOrDefault();
        }
    }
}