using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using Boyner.CaseStudy.ApplicationCore.Interfaces.Data;
using System.Threading.Tasks;
using Boyner.CaseStudy.ApplicationCore.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Boyner.CaseStudy.Infrastructure.Data
{
    public class MongoRepository<T> : IMongoRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IOptions<DefaultMongoDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        public async Task<List<T>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();
            
        public async Task<List<T>> GetPagedListAsync(int page, int pageSize) =>
            await _collection.Find(_ => true).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

        public async Task<List<T>> GetPagedListAsync(int page, int pageSize, Expression<Func<T, object>> sortBy)  =>
            await _collection.Find(_ => true).Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .SortBy(sortBy)
            .ToListAsync();

        public async Task<T> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(T newBook) =>
            await _collection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, T updatedBook) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
