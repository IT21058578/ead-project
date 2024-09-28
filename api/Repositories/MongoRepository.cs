using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Repositories
{
    public class MongoRepository<T>(AppDbContext dbContext) : IMongoRepository<T> where T : class, IMongoModel
    {
        private readonly AppDbContext dbContext = dbContext;
        private readonly DbSet<T> dbSet = dbContext.Set<T>();

        // Get an entity by its string Id
        public T? GetById(ObjectId id)
        {
            return dbSet.FirstOrDefault(e => e.Id == id); // Assuming Id is a string in IMongoModel
        }

        public T? GetById(String id)
        {
            return this.GetById(new ObjectId(id)); // Assuming Id is a string in IMongoModel
        }

        // Get all entities
        public IEnumerable<T> GetAll()
        {
            return dbSet.AsNoTracking().ToList(); // AsNoTracking for better performance on read-only queries
        }

        // Add a single entity
        public void Add(T entity)
        {
            dbSet.Add(entity);

            dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(dbContext.ChangeTracker.DebugView.LongView);

            dbContext.SaveChanges();
        }

        // Add multiple entities
        public void AddMany(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
            dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(dbContext.ChangeTracker.DebugView.LongView);
            dbContext.SaveChanges();
        }

        // Update a single entity
        public void Update(T entity)
        {
            var entityToUpdate = dbSet.FirstOrDefault(e => e.Id == entity.Id);
            if (entityToUpdate != null)
            {
                dbSet.Update(entity);
                dbContext.ChangeTracker.DetectChanges();
                Console.WriteLine(dbContext.ChangeTracker.DebugView.LongView);
                dbContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Entity with ID {entity.Id} cannot be found.");
            }
        }

        // Update multiple entities
        public void UpdateMany(IEnumerable<T> entities)
        {
            dbSet.UpdateRange(entities);
            dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(dbContext.ChangeTracker.DebugView.LongView);
            dbContext.SaveChanges();
        }

        // Delete a single entity
        public void Delete(T entity)
        {
            var entityToDelete = dbSet.FirstOrDefault(e => e.Id == entity.Id);
            if (entityToDelete != null)
            {
                dbSet.Remove(entityToDelete);
                dbContext.ChangeTracker.DetectChanges();
                Console.WriteLine(dbContext.ChangeTracker.DebugView.LongView);
                dbContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Entity with ID {entity.Id} cannot be found.");
            }
        }

        // Delete multiple entities
        public void DeleteMany(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(dbContext.ChangeTracker.DebugView.LongView);
            dbContext.SaveChanges();
        }
    }
}