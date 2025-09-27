using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Application.Common.Interfaces;
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
            _collection = _context.GetDatabase().GetCollection<T>(collectionName.ToLower());
        }

        public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(predicate).ToListAsync(cancellationToken);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(predicate).AnyAsync(cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return (int)await _collection.CountDocumentsAsync(predicate, cancellationToken: cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
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
            await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.DeleteOneAsync(filter, cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
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

            await DeleteAsync(id, cancellationToken);
        }
    }
}