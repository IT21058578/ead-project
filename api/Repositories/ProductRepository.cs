using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services;

namespace api.Repositories
{
    public class ProductRepository(AppDbContext dbContext) : MongoRepository<Product>(dbContext)
    {
    }
}