using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Configurations;
using api.Utilities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Repositories
{
    public class MongoRepository<T>(AppDbContext dbContext) : IMongoRepository<T> where T : BaseModel
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly DbSet<T> _dbSet = dbContext.Set<T>();

        public T? GetById(ObjectId id)
        {
            return _dbSet.FirstOrDefault(e => e.Id == id); // Assuming Id is a string in IMongoModel
        }

        public T? GetById(String id)
        {
            return this.GetById(new ObjectId(id)); // Assuming Id is a string in IMongoModel
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList(); // AsNoTracking for better performance on read-only queries
        }

        public T Add(T entity)
        {
            _dbSet.Add(entity);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            _dbContext.SaveChanges();
            return entity;
        }

        public IEnumerable<T> AddMany(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            _dbContext.SaveChanges();
            return entities;
        }

        public T Update(T entity)
        {
            var entityToUpdate = _dbSet.FirstOrDefault(e => e.Id == entity.Id);
            if (entityToUpdate != null)
            {
                _dbSet.Update(entity);
                _dbContext.ChangeTracker.DetectChanges();
                Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
                _dbContext.SaveChanges();
                return entity;
            }
            else
            {
                throw new ArgumentException($"Entity with ID {entity.Id} cannot be found.");
            }
        }

        public IEnumerable<T> UpdateMany(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            _dbContext.SaveChanges();
            return entities;
        }

        public T Delete(T entity)
        {
            var entityToDelete = _dbSet.FirstOrDefault(e => e.Id == entity.Id);
            if (entityToDelete != null)
            {
                _dbSet.Remove(entityToDelete);
                _dbContext.ChangeTracker.DetectChanges();
                Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
                _dbContext.SaveChanges();
                return entity;
            }
            else
            {
                throw new ArgumentException($"Entity with ID {entity.Id} cannot be found.");
            }
        }

        public IEnumerable<T> DeleteMany(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            _dbContext.SaveChanges();
            return entities;
        }

        public Page<T> GetPage(PageRequest<T> pageRequest)
        {
            var query = pageRequest.Query;
            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageRequest.PageSize);
            var items = query.Skip((pageRequest.Page - 1) * pageRequest.PageSize).Take(pageRequest.PageSize).ToList();
            var isFirst = pageRequest.Page == 1;
            var isLast = pageRequest.Page == totalPages;

            return new Page<T>
            {
                Data = items,
                Meta = new Page<T>.Metadata
                {
                    Page = pageRequest.Page,
                    PageSize = pageRequest.PageSize,
                    Total = totalItems,
                    TotalPages = totalPages,
                    TotalInPage = items.Count,
                    IsFirst = isFirst,
                    IsLast = isLast
                }
            };
        }
    }
}