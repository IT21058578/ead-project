
using api.Models;
using api.Configurations;
using api.Utilities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace api.Repositories
{
    /// <summary>
    /// The MongoRepository class is a repository class that provides methods for interacting with the collection where entities are stored in the database.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <remarks>
    /// </remarks>
    public abstract class MongoRepository<T>(AppDbContext dbContext) : IMongoRepository<T> where T : BaseModel
    {
        private readonly AppDbContext _dbContext = dbContext;
        protected readonly DbSet<T> _dbSet = dbContext.Set<T>();

        // A repository method for finding an entity by its id asynchronously
        public async Task<T?> GetByIdAsync(ObjectId id)
        {
            return await _dbSet.FindAsync(id); // Assuming Id is a string in IMongoModel
        }

        // A repository method for finding an entity by its id asynchronously
        public async Task<T?> GetByIdAsync(string id)
        {
            return await this.GetByIdAsync(new ObjectId(id));
        }

        // A repository method for finding entities by its id
        public T? GetById(ObjectId id)
        {
            return _dbSet.Find(id); // Assuming Id is a string in IMongoModel
        }

        // A repository method for finding entities by its id
        public T? GetById(string id)
        {
            return this.GetById(new ObjectId(id)); // Assuming Id is a string in IMongoModel
        }

        // A repository method for finding entities by their ids
        public IEnumerable<T> GetByIds(IEnumerable<ObjectId> ids)
        {
            return _dbSet.Where(e => ids.Contains(e.Id)).ToList();
        }

        // A repository method for finding entities by their ids
        public IEnumerable<T> GetByIds(IEnumerable<string> ids)
        {
            var objectIds = ids.Select(id => new ObjectId(id));
            return _dbSet.Where(e => objectIds.Contains(e.Id)).ToList();
        }

        // A repository method for finding all entities
        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList(); // AsNoTracking for better performance on read-only queries
        }

        // A repository method for adding an entity to the collection
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

        // A repository method for adding an entity to the collection asynchronously
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

        // A repository method for adding multiple entities to the collection
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

        // A repository method for adding multiple entities to the collection asynchronously
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

        // A repository method for updating an entity in the collection
        public T Update(T entity)
        {
            var entityToUpdate = _dbSet.FirstOrDefault(e => e.Id == entity.Id);
            if (entityToUpdate != null)
            {
                _dbContext.Entry(entityToUpdate).State = EntityState.Detached;
                _dbContext.Entry(entity).State = EntityState.Modified;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedBy = ObjectId.Empty; // TODO: Get id from session
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

        // A repository method for updating an entity in the collection asynchronously
        public async Task<T> UpdateAsync(T entity)
        {
            var entityToUpdate = await _dbSet.FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (entityToUpdate != null)
            {
                _dbContext.Entry(entityToUpdate).State = EntityState.Detached;
                _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
                entityToUpdate.UpdatedAt = DateTime.UtcNow;
                entityToUpdate.UpdatedBy = ObjectId.Empty; // TODO: Get id from session
                _dbSet.Update(entity);
                _dbContext.ChangeTracker.DetectChanges();
                Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            else
            {
                throw new ArgumentException($"Entity with ID {entity.Id} cannot be found.");
            }
        }

        // A repository method for updating multiple entities in the collection
        public IEnumerable<T> UpdateMany(IEnumerable<T> entities)
        {
            entities.ToList().ForEach(e =>
            {
                if (e.Id == ObjectId.Empty)
                {
                    // Don't let these objects save
                    throw new ArgumentException("Entity ID must not be empty.");
                }
                _dbContext.Entry(e).State = EntityState.Modified;
                e.CreatedBy = ObjectId.Empty; // TODO: Get id from session
                e.CreatedAt = DateTime.UtcNow;
            });
            _dbSet.UpdateRange(entities);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            _dbContext.SaveChanges();
            return entities;
        }

        // A repository method for updating multiple entities in the collection asynchronously
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

        // A repository method for deleting an entity from the collection
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

        // A repository method for deleting an entity from the collection
        public T Delete(string id)
        {
            return this.Delete(new ObjectId(id));
        }

        // A repository method for deleting multiple entities from the collection
        public IEnumerable<T> DeleteMany(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            _dbContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_dbContext.ChangeTracker.DebugView.LongView);
            _dbContext.SaveChanges();
            return entities;
        }

        // A repository method for conducting pagination on the collection
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