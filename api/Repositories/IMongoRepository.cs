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
        T Add(T entity);
        IEnumerable<T> AddMany(IEnumerable<T> entities);
        T Update(T entity);
        IEnumerable<T> UpdateMany(IEnumerable<T> entities);
        T Delete(T entity);
        IEnumerable<T> DeleteMany(IEnumerable<T> entities);
        Page<T> GetPage(PageRequest<T> pageRequest);
    }
}