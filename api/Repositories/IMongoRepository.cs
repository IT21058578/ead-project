
using api.Models;
using api.Utilities;
using MongoDB.Bson;

namespace api.Repositories
{
    /// <summary>
    /// The IMongoRepository interface provides methods for interacting with the collection
    /// where entities of type T are stored in the database.
    /// </summary>
    /// 
    /// <typeparam name="T">The type of entity.</typeparam>
    /// 
    /// <remarks>
    /// The IMongoRepository interface provides methods for interacting with the collection
    /// where entities of type T are stored in the database. It defines methods for CRUD
    /// operations such as GetById, GetAll, Add, Update, and Delete. It also provides a
    /// method for retrieving a page of entities based on a page request.
    /// </remarks>
    public interface IMongoRepository<T> where T : BaseModel
    {
        T? GetById(string id);
        T? GetById(ObjectId id);
        IEnumerable<T> GetAll();
        T Add(T entity);
        IEnumerable<T> AddMany(IEnumerable<T> entities);
        T Update(T entity);
        IEnumerable<T> UpdateMany(IEnumerable<T> entities);
        T Delete(string id);
        T Delete(ObjectId entity);
        IEnumerable<T> DeleteMany(IEnumerable<T> entities);
        Page<T> GetPage(PageRequest<T> pageRequest);
    }
}