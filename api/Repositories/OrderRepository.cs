
using api.Configurations;
using api.Models;

namespace api.Repositories
{
    /// <summary>
    /// The OrderRepository class used to interact with the Order Entity
    /// </summary>
    /// 
    /// <remarks>
    /// The OrderRepository class is a repository class that inherits the MongoRepository 
    /// class and implements the MongoRepository<T> interface. It provides methods for
    /// interacting with the collection where Orders are stored in the database.
    /// </remarks>
    public class OrderRepository(AppDbContext dbContext) : MongoRepository<Order>(dbContext)
    {
    }
}