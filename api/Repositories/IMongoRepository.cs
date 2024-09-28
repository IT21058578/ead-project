using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Utilities;
using MongoDB.Bson;

namespace api.Repositories
{
    public interface IMongoRepository<T> where T : class
    {
        T? GetById(string id);
        T? GetById(ObjectId id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void AddMany(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateMany(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteMany(IEnumerable<T> entities);
        Page<T> GetPage(PageRequest<T> pageRequest);
    }
}