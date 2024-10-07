using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Configurations;
using api.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace api.Repositories
{
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