using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Configurations;
using api.Models;
using MongoDB.Bson;

namespace api.Repositories
{
  public class CredentialRepository(AppDbContext dbContext) : MongoRepository<Credential>(dbContext)
  {
    public Credential? FindByUserId(ObjectId userId)
    {
      return _dbSet.Where(c => c.UserId == userId).FirstOrDefault();
    }
  }
}