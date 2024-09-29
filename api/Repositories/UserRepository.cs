using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Configurations;
using api.Models;

namespace api.Repositories
{
    public class UserRepository(AppDbContext dbContext) : MongoRepository<User>(dbContext)
    {
        public User? FindByEmail(string email)
        {
            return _dbSet.Where(o => o.Email == email).FirstOrDefault();
        }
    }
}