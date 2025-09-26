using System;
using System.Threading.Tasks;
using VehicleShowroomManagement.Infrastructure.Interfaces;
using VehicleShowroomManagement.Infrastructure.Persistence;

namespace VehicleShowroomManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Unit of Work implementation for managing database transactions
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VehicleShowroomDbContext _context;
        private bool _disposed = false;

        public UnitOfWork(VehicleShowroomDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await _context.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}