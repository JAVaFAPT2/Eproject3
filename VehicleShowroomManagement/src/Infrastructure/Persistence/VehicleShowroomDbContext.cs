using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Infrastructure.Persistence
{
    /// <summary>
    /// MongoDB database context for Vehicle Showroom Management System
    /// Implements Unit of Work pattern
    /// </summary>
    public class VehicleShowroomDbContext : IUnitOfWork
    {
        private readonly IMongoDatabase _database;
        private IClientSessionHandle _session;

        public VehicleShowroomDbContext(IMongoDatabase database)
        {
            _database = database;
        }

        // MongoDB Collections
        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
        public IMongoCollection<Role> Roles => _database.GetCollection<Role>("roles");
        public IMongoCollection<UserRole> UserRoles => _database.GetCollection<UserRole>("userRoles");
        public IMongoCollection<Brand> Brands => _database.GetCollection<Brand>("brands");
        public IMongoCollection<Model> Models => _database.GetCollection<Model>("models");
        public IMongoCollection<Vehicle> Vehicles => _database.GetCollection<Vehicle>("vehicles");
        public IMongoCollection<VehicleImage> VehicleImages => _database.GetCollection<VehicleImage>("vehicleImages");
        public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>("customers");
        public IMongoCollection<SalesOrder> SalesOrders => _database.GetCollection<SalesOrder>("salesOrders");
        public IMongoCollection<SalesOrderItem> SalesOrderItems => _database.GetCollection<SalesOrderItem>("salesOrderItems");
        public IMongoCollection<Invoice> Invoices => _database.GetCollection<Invoice>("invoices");
        public IMongoCollection<Payment> Payments => _database.GetCollection<Payment>("payments");
        public IMongoCollection<ServiceOrder> ServiceOrders => _database.GetCollection<ServiceOrder>("serviceOrders");

        // Helper methods
        public IMongoDatabase GetDatabase()
        {
            return _database;
        }

        // IUnitOfWork implementation
        public async Task<int> SaveChangesAsync()
        {
            if (_session == null)
            {
                throw new InvalidOperationException("No active session. Call BeginTransactionAsync() first.");
            }

            // In MongoDB, changes are automatically saved
            // We just need to ensure the session is still active
            return 1;
        }

        public async Task BeginTransactionAsync()
        {
            var client = _database.Client;
            _session = await client.StartSessionAsync();
            _session.StartTransaction();
        }

        public async Task CommitTransactionAsync()
        {
            if (_session == null)
            {
                throw new InvalidOperationException("No active session.");
            }

            await _session.CommitTransactionAsync();
            _session.Dispose();
            _session = null;
        }

        public async Task RollbackTransactionAsync()
        {
            if (_session == null)
            {
                throw new InvalidOperationException("No active session.");
            }

            await _session.AbortTransactionAsync();
            _session.Dispose();
            _session = null;
        }

        public void Dispose()
        {
            _session?.Dispose();
        }

        // Initialize collections with indexes
        public async Task InitializeCollectionsAsync()
        {
            var collections = new[]
            {
                (Users, "users"),
                (Roles, "roles"),
                (UserRoles, "userRoles"),
                (Brands, "brands"),
                (Models, "models"),
                (Vehicles, "vehicles"),
                (VehicleImages, "vehicleImages"),
                (Customers, "customers"),
                (SalesOrders, "salesOrders"),
                (SalesOrderItems, "salesOrderItems"),
                (Invoices, "invoices"),
                (Payments, "payments"),
                (ServiceOrders, "serviceOrders")
            };

            foreach (var (collection, name) in collections)
            {
                await CreateIndexesAsync(collection, name);
            }
        }

        private async Task CreateIndexesAsync<T>(IMongoCollection<T> collection, string collectionName)
        {
            var indexModels = new List<CreateIndexModel<T>>();

            // Add common indexes based on collection type
            switch (collectionName)
            {
                case "users":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("email"),
                        new CreateIndexOptions { Unique = true }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("username"),
                        new CreateIndexOptions { Unique = true }
                    ));
                    break;

                case "vehicles":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("vin"),
                        new CreateIndexOptions { Unique = true }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("status")
                    ));
                    break;

                case "roles":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("roleName"),
                        new CreateIndexOptions { Unique = true }
                    ));
                    break;

                case "customers":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("email"),
                        new CreateIndexOptions { Unique = true }
                    ));
                    break;

                case "brands":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("brandName"),
                        new CreateIndexOptions { Unique = true }
                    ));
                    break;

                case "models":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Combine(
                            Builders<T>.IndexKeys.Ascending("modelName"),
                            Builders<T>.IndexKeys.Ascending("brandId")
                        ),
                        new CreateIndexOptions { Unique = true }
                    ));
                    break;
            }

            if (indexModels.Any())
            {
                await collection.Indexes.CreateManyAsync(indexModels);
            }
        }
    }
}