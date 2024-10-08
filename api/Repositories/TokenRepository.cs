using api.Configurations;
using api.Models;
using api.Utilities;

namespace api.Repositories
{
    /// <summary>
    /// The TokenRepository class used to interact with the Token Entity
    /// </summary>
    /// 
    /// <remarks>
    /// The TokenRepository class is a repository class that inherits the MongoRepository 
    /// class and implements the MongoRepository<T> interface. It provides methods for
    /// interacting with the collection where Tokens are stored in the database.
    /// </remarks>
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