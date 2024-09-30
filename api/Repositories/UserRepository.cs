using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Configurations;
using api.Models;
using api.Utilities;

namespace api.Repositories
{
    public class UserRepository(AppDbContext dbContext) : MongoRepository<User>(dbContext)
    {
        public User? FindByEmail(string email)
        {
            return _dbSet.Where(o => o.Email == email).FirstOrDefault();
        }

        public IEnumerable<User> FindByRole(AppUserRole role)
        {
            return _dbSet.Where(o => o.Role == role).ToList();
        }
    }
}