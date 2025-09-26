using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Infrastructure.Interfaces
{
    /// <summary>
    /// Generic repository interface for domain entities
    /// </summary>
    public interface IRepository<T> where T : IEntity
    {
        /// <summary>
        /// Gets all entities as IQueryable for further filtering
        /// </summary>
        IQueryable<T> GetAllQueryable();

        /// <summary>
        /// Gets all entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets entity by ID
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Finds first entity matching the predicate
        /// </summary>
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds a new entity
        /// </summary>
        Task AddAsync(T entity);

        /// <summary>
        /// Adds multiple entities
        /// </summary>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Updates an entity
        /// </summary>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Updates multiple entities
        /// </summary>
        Task UpdateRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Removes an entity
        /// </summary>
        Task RemoveAsync(T entity);

        /// <summary>
        /// Removes multiple entities
        /// </summary>
        Task RemoveRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Checks if any entity matches the predicate
        /// </summary>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Counts entities matching the predicate
        /// </summary>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Saves all changes to the database
        /// </summary>
        Task SaveChangesAsync();
    }
}