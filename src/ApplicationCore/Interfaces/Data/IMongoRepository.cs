using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Boyner.CaseStudy.ApplicationCore.Interfaces.Data
{
    public interface IMongoRepository<T>
    {
        Task<List<T>> GetAsync();
        Task<List<T>> GetPagedListAsync(int page, int pageSize);
        Task<List<T>> GetPagedListAsync(int page, int pageSize, Expression<Func<T, object>> sortByDescending);
        Task<T> GetAsync(string id);
        Task CreateAsync(T newBook);
        Task UpdateAsync(string id, T updatedBook);
        Task RemoveAsync(string id);
    }
}