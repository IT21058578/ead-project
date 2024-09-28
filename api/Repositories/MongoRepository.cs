using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Repositories
{
    public class MongoRepository<T>(IMongoDatabase db, string collectionName) : IMongoRepository<T> where T : class, IMongoModel
    {
        private readonly IMongoCollection<T> _collection = db.GetCollection<T>(collectionName);

        public T GetById(string id)
        {
            return _collection.Find<T>(Builders<T>.Filter.Eq("_id", new ObjectId(id))).FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return _collection.Find<T>(new BsonDocument()).ToList();
        }

        public void Add(T entity)
        {
            _collection.InsertOne(entity);
        }

        public void Update(T entity)
        {
            _collection.ReplaceOne(Builders<T>.Filter.Eq("_id", new ObjectId(entity.Id)), entity);
        }

        public void Delete(T entity)
        {
            _collection.DeleteOne(Builders<T>.Filter.Eq("_id", new ObjectId(entity.Id)));
        }
    }
}