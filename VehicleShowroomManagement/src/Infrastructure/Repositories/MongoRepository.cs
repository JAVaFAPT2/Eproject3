using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Generic MongoDB repository implementation
    /// </summary>
    public class MongoRepository<T> : IRepository<T> where T : class
    {
        protected readonly VehicleShowroomDbContext _context;
        protected readonly IMongoCollection<T> _collection;

        public MongoRepository(VehicleShowroomDbContext context, string collectionName)
        {
            _context = context;
            _collection = _context.GetDatabase().GetCollection<T>(collectionName.ToUpper());
        }

        public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            // Handle ObjectId conversion for MongoDB
            MongoDB.Bson.ObjectId objectId;
            if (MongoDB.Bson.ObjectId.TryParse(id, out objectId))
            {
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                // Fallback to string ID if it's not a valid ObjectId
                var filter = Builders<T>.Filter.Eq("_id", id);
                return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            }
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

    public async Task<int> CountAsync(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        // Cast to IMongoQueryable to access MongoDB async extensions
        if (queryable is IMongoQueryable<T> mongoQueryable)
        {
            return await mongoQueryable.CountAsync(cancellationToken);
        }
        
        // Fallback: convert to list and count (less efficient)
        var list = queryable.ToList();
        return list.Count;
    }

    public async Task<IEnumerable<T>> GetPagedAsync(IQueryable<T> queryable, int skip, int take, CancellationToken cancellationToken = default)
    {
        // Cast to IMongoQueryable to access MongoDB async extensions
        if (queryable is IMongoQueryable<T> mongoQueryable)
        {
            return await mongoQueryable.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }
        
        // Fallback: use synchronous operations (less efficient)
        return queryable.Skip(skip).Take(take).ToList();
    }

        public IQueryable<T> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            // For MongoDB, we need to get the ID from the entity
            // First try to find property with [BsonId] attribute, then fallback to "Id"
            var idProperty = typeof(T).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttributes(typeof(MongoDB.Bson.Serialization.Attributes.BsonIdAttribute), false).Any())
                ?? typeof(T).GetProperty("Id");
                
            if (idProperty == null)
            {
                throw new InvalidOperationException("Entity must have an Id property or a property marked with [BsonId]");
            }

            var id = idProperty.GetValue(entity)?.ToString();
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidOperationException("Entity must have a valid Id");
            }

            // Handle ObjectId conversion for MongoDB
            MongoDB.Bson.ObjectId objectId;
            if (MongoDB.Bson.ObjectId.TryParse(id, out objectId))
            {
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                var result = await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
                
                if (result.MatchedCount == 0)
                {
                    throw new InvalidOperationException($"No document found with ObjectId: {id}");
                }
            }
            else
            {
                // Fallback to string ID if it's not a valid ObjectId
                var filter = Builders<T>.Filter.Eq("_id", id);
                var result = await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
                
                if (result.MatchedCount == 0)
                {
                    throw new InvalidOperationException($"No document found with String ID: {id}");
                }
            }
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.DeleteOneAsync(filter, cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            // For MongoDB, we need to get the ID from the entity
            // First try to find property with [BsonId] attribute, then fallback to "Id"
            var idProperty = typeof(T).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttributes(typeof(MongoDB.Bson.Serialization.Attributes.BsonIdAttribute), false).Any())
                ?? typeof(T).GetProperty("Id");
                
            if (idProperty == null)
            {
                throw new InvalidOperationException("Entity must have an Id property or a property marked with [BsonId]");
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