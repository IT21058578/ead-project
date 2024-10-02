using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Configurations;
using api.Models;
using MongoDB.Bson;

namespace api.Repositories
{
	public class ReviewRepository(AppDbContext dbContext) : MongoRepository<Review>(dbContext)
	{
		public IEnumerable<Review> GetByVendorId(string vendorId)
		{
			return _dbSet.Where(r => r.VendorId == new ObjectId(vendorId)).ToList();
		}

		public IEnumerable<Review> GetByProductId(string productId)
		{
			return _dbSet.Where(r => r.ProductId == new ObjectId(productId)).ToList();
		}
	}
}