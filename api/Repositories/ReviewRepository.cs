using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Configurations;
using api.Models;

namespace api.Repositories
{
	public class ReviewRepository(AppDbContext dbContext) : MongoRepository<Review>(dbContext)
	{
	}
}