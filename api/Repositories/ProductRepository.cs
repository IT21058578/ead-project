using api.Configurations;
using api.Models;

namespace api.Repositories
{
    /// <summary>
    /// The ProductRepository class used to interact with the Product Entity
    /// </summary>
    /// 
    /// <remarks>
    /// The ProductRepository class is a repository class that inherits the MongoRepository 
    /// class and implements the MongoRepository<T> interface. It provides methods for
    /// interacting with the collection where Products are stored in the database.
    /// </remarks>
    public class ProductRepository(AppDbContext dbContext) : MongoRepository<Product>(dbContext)
    {
    }
}