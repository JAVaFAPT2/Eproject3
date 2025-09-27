using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Infrastructure.Persistence
{
    /// <summary>
    /// MongoDB implementation of Unit of Work
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoDatabase _database;
        private IClientSessionHandle? _session;
        private bool _disposed = false;

        public UnitOfWork(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // In MongoDB, changes are automatically saved when operations are performed
            // This method is here for consistency with the interface
            return await Task.FromResult(1);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_session == null)
            {
                _session = await _database.Client.StartSessionAsync(cancellationToken: cancellationToken);
                _session.StartTransaction();
            }
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_session != null)
            {
                await _session.CommitTransactionAsync(cancellationToken);
                _session.Dispose();
                _session = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_session != null)
            {
                await _session.AbortTransactionAsync(cancellationToken);
                _session.Dispose();
                _session = null;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _session?.Dispose();
                _disposed = true;
            }
        }
    }
}