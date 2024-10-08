using api.Configurations;
using api.Models;
using api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    /// <summary>
    /// The UserRepository class used to interact with the User Entity
    /// </summary>
    /// 
    /// <remarks>
    /// The UserRepository class is a repository class that inherits the MongoRepository 
    /// class and implements the MongoRepository<T> interface. It provides methods for
    /// interacting with the collection where Users are stored in the database.
    /// </remarks>
    public class UserRepository(AppDbContext dbContext) : MongoRepository<User>(dbContext)
    {
        // A repository method for finding user by the email
        public User? FindByEmail(string email)
        {
            return _dbSet.Where(o => o.Email == email).FirstOrDefault();
        }

        // A repository method for finding user by the email asynchronously
        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _dbSet.Where(o => o.Email == email).FirstOrDefaultAsync();
        }

        // A repository method for finding users by their role
        public IEnumerable<User> FindByRole(AppUserRole role)
        {
            return _dbSet.Where(o => o.Role == role).ToList();
        }
    }
}