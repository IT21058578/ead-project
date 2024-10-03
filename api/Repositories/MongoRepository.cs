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
        protected readonly DbSet<T> _dbSet = dbContext.Set<T>();

        public Task<T?> GetByIdAsync(ObjectId id)
        {
            return _dbSet.FirstOrDefaultAsync(e => e.Id == id); // Assuming Id is a string in IMongoModel
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await this.GetByIdAsync(new ObjectId(id));
        }


        public T? GetById(ObjectId id)
        {
            return _dbSet.FirstOrDefault(e => e.Id == id); // Assuming Id is a string in IMongoModel
        }

        public T? GetById(string id)
        {
            return this.GetById(new ObjectId(id)); // Assuming Id is a string in IMongoModel
        }

        public IEnumerable<T> GetByIds(IEnumerable<ObjectId> ids)
        {
            return _dbSet.Where(e => ids.Contains(e.Id)).ToList();
        }

        public IEnumerable<T> GetByIds(IEnumerable<string> ids)
        {
            var objectIds = ids.Select(id => new ObjectId(id));
            return _dbSet.Where(e => objectIds.Contains(e.Id)).ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList(); // AsNoTracking for better performance on read-only queries
        }

        public T Add(T entity)
        {
            if (entity.Id != ObjectId.Empty)
            {
                // Don't let these objects save
                throw new ArgumentException("Entity ID must be empty.");
            }
            entity.CreatedBy = ObjectId.Empty; // TODO: Get id from session
            entity.CreatedAt = DateTime.UtcNow;
            _dbSet.Add(entity);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            _dbContext.SaveChanges();
            Console.WriteLine(entity);
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity.Id != ObjectId.Empty)
            {
                // Don't let these objects save
                throw new ArgumentException("Entity ID must be empty.");
            }
            entity.CreatedBy = ObjectId.Empty; // TODO: Get id from session
            entity.CreatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            _dbContext.SaveChanges();
            Console.WriteLine(entity);
            return entity;
        }

        public IEnumerable<T> AddMany(IEnumerable<T> entities)
        {
            entities.ToList().ForEach(e =>
            {
                if (e.Id != ObjectId.Empty)
                {
                    // Don't let these objects save
                    throw new ArgumentException("Entity ID must be empty.");
                }
                e.CreatedBy = ObjectId.Empty; // TODO: Get id from session
                e.CreatedAt = DateTime.UtcNow;
            });
            _dbSet.AddRange(entities);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            _dbContext.SaveChanges();
            return entities;
        }

        public Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> entities)
        {
            entities.ToList().ForEach(e =>
            {
                if (e.Id != ObjectId.Empty)
                {
                    // Don't let these objects save
                    throw new ArgumentException("Entity ID must be empty.");
                }
                e.CreatedBy = ObjectId.Empty; // TODO: Get id from session
                e.CreatedAt = DateTime.UtcNow;
            });
            _dbSet.AddRange(entities);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            _dbContext.SaveChanges();
            return Task.FromResult(entities);
        }

        public T Update(T entity)
        {
            var entityToUpdate = _dbSet.FirstOrDefault(e => e.Id == entity.Id);
            if (entityToUpdate != null)
            {
                entityToUpdate.UpdatedAt = DateTime.UtcNow;
                entityToUpdate.UpdatedBy = ObjectId.Empty; // TODO: Get id from session
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

        public Task<T> UpdateAsync(T entity)
        {
            var entityToUpdate = _dbSet.FirstOrDefault(e => e.Id == entity.Id);
            if (entityToUpdate != null)
            {
                entityToUpdate.UpdatedAt = DateTime.UtcNow;
                entityToUpdate.UpdatedBy = ObjectId.Empty; // TODO: Get id from session
                _dbSet.Update(entity);
                _dbContext.ChangeTracker.DetectChanges();
                Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
                _dbContext.SaveChanges();
                return Task.FromResult(entity);
            }
            else
            {
                throw new ArgumentException($"Entity with ID {entity.Id} cannot be found.");
            }
        }

        public IEnumerable<T> UpdateMany(IEnumerable<T> entities)
        {
            entities.ToList().ForEach(e =>
            {
                if (e.Id == ObjectId.Empty)
                {
                    // Don't let these objects save
                    throw new ArgumentException("Entity ID must not be empty.");
                }
                e.CreatedBy = ObjectId.Empty; // TODO: Get id from session
                e.CreatedAt = DateTime.UtcNow;
            });
            _dbSet.UpdateRange(entities);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            _dbContext.SaveChanges();
            return entities;
        }

        public Task<IEnumerable<T>> UpdateManyAsync(IEnumerable<T> entities)
        {
            entities.ToList().ForEach(e =>
            {
                if (e.Id == ObjectId.Empty)
                {
                    // Don't let these objects save
                    throw new ArgumentException("Entity ID must not be empty.");
                }
                e.CreatedBy = ObjectId.Empty; // TODO: Get id from session
                e.CreatedAt = DateTime.UtcNow;
            });
            _dbSet.UpdateRange(entities);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            _dbContext.SaveChanges();
            return Task.FromResult(entities);
        }

        public T Delete(ObjectId id)
        {
            var entityToDelete = _dbSet.FirstOrDefault(e => e.Id == id);
            if (entityToDelete != null)
            {
                _dbSet.Remove(entityToDelete);
                _dbContext.ChangeTracker.DetectChanges();
                Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
                _dbContext.SaveChanges();
                return entityToDelete;
            }
            else
            {
                throw new ArgumentException($"Entity with ID {id} cannot be found.");
            }
        }

        public T Delete(string id)
        {
            return this.Delete(new ObjectId(id));
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
            // Start with the DbSet<T> as the base query
            var query = _dbSet.AsQueryable();

            // Apply the filter expression, if any
            if (pageRequest.FilterExpression != null)
            {
                query = query.Where(pageRequest.FilterExpression);
            }

            // Apply the sorting expression, if any
            if (pageRequest.SortExpression != null)
            {
                query = pageRequest.SortExpression(query);
            }

            // Get the total count before applying pagination
            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageRequest.PageSize);

            // Apply pagination (Skip and Take)
            var items = query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToList();

            var isFirst = pageRequest.Page == 1;
            var isLast = pageRequest.Page == totalPages;

            // Return the paginated result as a Page<T>
            return new Page<T>
            {
                Data = items,
                Meta = new PageMetadata
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