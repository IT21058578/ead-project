
using api.Configurations;
using api.Models;
using MongoDB.Bson;

namespace api.Repositories
{
	/// <summary>
	/// The ReviewRepository class used to interact with the Review Entity
	/// </summary>
	/// 
	/// <remarks>
	/// The ReviewRepository class is a repository class that inherits the MongoRepository 
	/// class and implements the MongoRepository<T> interface. It provides methods for
	/// interacting with the collection where Reviews are stored in the database.
	/// </remarks>
	public class ReviewRepository(AppDbContext dbContext) : MongoRepository<Review>(dbContext)
	{
		// A repository method for finding reviews by the vendor id
		public IEnumerable<Review> GetByVendorId(string vendorId)
		{
			return _dbSet.Where(r => r.VendorId == new ObjectId(vendorId)).ToList();
		}

		// A repository method for finding reviews by the product id
		public IEnumerable<Review> GetByProductId(string productId)
		{
			return _dbSet.Where(r => r.ProductId == new ObjectId(productId)).ToList();
		}
	}
}