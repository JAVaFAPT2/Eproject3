using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Infrastructure.Interfaces;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Generic MongoDB repository implementation
    /// </summary>
    public class MongoRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly VehicleShowroomDbContext _context;
        protected readonly IMongoCollection<T> _collection;

        public MongoRepository(VehicleShowroomDbContext context, string collectionName)
        {
            _context = context;
            _collection = context.GetType().GetProperty(collectionName)?.GetValue(context) as IMongoCollection<T>
                ?? throw new ArgumentException($"Collection {collectionName} not found in context");
        }

        public IMongoQueryable<T> GetAllQueryable()
        {
            return _collection.AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _collection.InsertManyAsync(entities);
        }

        public async Task UpdateAsync(T entity)
        {
            // For MongoDB, we need to get the ID from the entity
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
            {
                throw new InvalidOperationException("Entity must have an Id property");
            }

            var id = idProperty.GetValue(entity)?.ToString();
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidOperationException("Entity must have a valid Id");
            }

            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity);
            }
        }

        public async Task RemoveAsync(T entity)
        {
            // For MongoDB, we need to get the ID from the entity
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
            {
                throw new InvalidOperationException("Entity must have an Id property");
            }

            var id = idProperty.GetValue(entity)?.ToString();
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidOperationException("Entity must have a valid Id");
            }

            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            var ids = entities.Select(e =>
            {
                var idProperty = typeof(T).GetProperty("Id");
                return idProperty?.GetValue(e)?.ToString();
            }).Where(id => !string.IsNullOrEmpty(id)).ToList();

            if (ids.Any())
            {
                var filter = Builders<T>.Filter.In("_id", ids);
                await _collection.DeleteManyAsync(filter);
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).AnyAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return (int)await _collection.CountDocumentsAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            // MongoDB saves changes immediately, but we need to implement Unit of Work
            await _context.SaveChangesAsync();
        }
    }
}