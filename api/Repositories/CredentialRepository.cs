using api.Configurations;
using api.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace api.Repositories
{
  /// <summary>
  /// The CredentialRepository class used to interact with the Credential Entity
  /// </summary>
  /// 
  /// <remarks>
  /// The CredentialRepository class is a repository class that inherits the MongoRepository 
  /// class and implements the MongoRepository<T> interface. It provides methods for
  /// interacting with the collection where Credentials are stored in the database.
  /// </remarks>
  public class CredentialRepository(AppDbContext dbContext) : MongoRepository<Credential>(dbContext)
  {
    // A repository method for finding credential by the user id
    public Credential? FindByUserId(ObjectId userId)
    {
      return _dbSet.Where(c => c.UserId == userId).FirstOrDefault();
    }

    // A repository method for finding credential by the user id asynchronously
    public async Task<Credential?> FindByUserIdAsync(ObjectId userId)
    {
      return await _dbSet.Where(c => c.UserId == userId).FirstOrDefaultAsync();
    }
  }
}