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
            var collectionNames = new (string, dynamic)[]
            {
                ("users", Users),
                ("roles", Roles),
                ("userRoles", UserRoles),
                ("brands", Brands),
                ("models", Models),
                ("vehicles", Vehicles),
                ("vehicleImages", VehicleImages),
                ("customers", Customers),
                ("salesOrders", SalesOrders),
                ("salesOrderItems", SalesOrderItems),
                ("invoices", Invoices),
                ("payments", Payments),
                ("serviceOrders", ServiceOrders)
            };

            foreach (var (name, collection) in collectionNames)
            {
                await CreateIndexesAsync(collection, name);
            }
        }

        private async Task CreateIndexesAsync<T>(IMongoCollection<T> collection, string collectionName)
        {
            var indexModels = new List<CreateIndexModel<T>>();

            // Add comprehensive indexes based on collection type and query patterns
            switch (collectionName)
            {
                case "users":
                    // Authentication indexes (highest priority)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("email"),
                        new CreateIndexOptions { Unique = true, Name = "Email_Unique" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("username"),
                        new CreateIndexOptions { Unique = true, Name = "Username_Unique" }
                    ));

                    // Query optimization indexes
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("roleId").Ascending("isDeleted"),
                        new CreateIndexOptions { Name = "RoleId_IsDeleted" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("isActive").Ascending("isDeleted"),
                        new CreateIndexOptions { Name = "IsActive_IsDeleted" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("createdAt"),
                        new CreateIndexOptions { Name = "CreatedAt" }
                    ));
                    break;

                case "vehicles":
                    // Critical business indexes (VIN is primary identifier)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("vin"),
                        new CreateIndexOptions { Unique = true, Name = "VIN_Unique" }
                    ));

                    // Search and filter indexes (most frequent queries)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("status").Ascending("isDeleted"),
                        new CreateIndexOptions { Name = "Status_IsDeleted" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("modelId").Ascending("status"),
                        new CreateIndexOptions { Name = "ModelId_Status" }
                    ));

                    // Price-based queries (frequent for sales)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("price"),
                        new CreateIndexOptions { Name = "Price" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("year").Descending("price"),
                        new CreateIndexOptions { Name = "Year_Desc_Price" }
                    ));

                    // Audit trail (soft delete optimization)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("isDeleted").Ascending("createdAt"),
                        new CreateIndexOptions { Name = "IsDeleted_CreatedAt" }
                    ));
                    break;

                case "roles":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("roleName"),
                        new CreateIndexOptions { Unique = true, Name = "RoleName_Unique" }
                    ));
                    break;

                case "customers":
                    // Contact indexes (business critical)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("email"),
                        new CreateIndexOptions { Unique = true, Name = "Email_Unique" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("phone"),
                        new CreateIndexOptions { Name = "Phone" }
                    ));

                    // Query optimization
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("isDeleted").Ascending("createdAt"),
                        new CreateIndexOptions { Name = "IsDeleted_CreatedAt" }
                    ));
                    break;

                case "brands":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("brandName"),
                        new CreateIndexOptions { Unique = true, Name = "BrandName_Unique" }
                    ));
                    break;

                case "models":
                    // Composite unique index for model and brand relationship
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Combine(
                            Builders<T>.IndexKeys.Ascending("modelName"),
                            Builders<T>.IndexKeys.Ascending("brandId")
                        ),
                        new CreateIndexOptions { Unique = true, Name = "ModelName_BrandId_Unique" }
                    ));

                    // Query optimization
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("brandId").Ascending("isDeleted"),
                        new CreateIndexOptions { Name = "BrandId_IsDeleted" }
                    ));
                    break;

                case "salesOrders":
                    // Business critical indexes (customer queries are most frequent)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("customerId").Ascending("status"),
                        new CreateIndexOptions { Name = "CustomerId_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("status").Ascending("orderDate"),
                        new CreateIndexOptions { Name = "Status_OrderDate" }
                    ));

                    // Date-based analytics (frequent reporting queries)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("orderDate").Descending("totalAmount"),
                        new CreateIndexOptions { Name = "OrderDate_Desc_TotalAmount" }
                    ));

                    // Audit trail
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("isDeleted").Ascending("createdAt"),
                        new CreateIndexOptions { Name = "IsDeleted_CreatedAt" }
                    ));
                    break;

                case "salesOrderItems":
                    // Relationship indexes (join-like queries)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("salesOrderId").Ascending("isDeleted"),
                        new CreateIndexOptions { Name = "SalesOrderId_IsDeleted" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("vehicleId").Ascending("isDeleted"),
                        new CreateIndexOptions { Name = "VehicleId_IsDeleted" }
                    ));

                    // Pricing analytics
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("unitPrice").Descending("lineTotal"),
                        new CreateIndexOptions { Name = "UnitPrice_Desc_LineTotal" }
                    ));
                    break;

                case "invoices":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("salesOrderId"),
                        new CreateIndexOptions { Name = "SalesOrderId" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("invoiceNumber"),
                        new CreateIndexOptions { Unique = true, Name = "InvoiceNumber_Unique" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("status").Ascending("invoiceDate"),
                        new CreateIndexOptions { Name = "Status_InvoiceDate" }
                    ));
                    break;

                case "payments":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("invoiceId").Ascending("paymentDate"),
                        new CreateIndexOptions { Name = "InvoiceId_PaymentDate" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("paymentMethod").Ascending("amount"),
                        new CreateIndexOptions { Name = "PaymentMethod_Amount" }
                    ));
                    break;

                case "serviceOrders":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("vehicleId").Ascending("status"),
                        new CreateIndexOptions { Name = "VehicleId_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("customerId").Ascending("serviceDate"),
                        new CreateIndexOptions { Name = "CustomerId_ServiceDate" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("serviceDate").Descending("totalCost"),
                        new CreateIndexOptions { Name = "ServiceDate_Desc_TotalCost" }
                    ));
                    break;

                case "vehicleImages":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("vehicleId").Ascending("imageType"),
                        new CreateIndexOptions { Name = "VehicleId_ImageType" }
                    ));
                    break;

                case "userRoles":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Combine(
                            Builders<T>.IndexKeys.Ascending("userId"),
                            Builders<T>.IndexKeys.Ascending("roleId")
                        ),
                        new CreateIndexOptions { Unique = true, Name = "UserId_RoleId_Unique" }
                    ));
                    break;
            }

            // Create indexes if any (with error handling and logging)
            if (indexModels.Any())
            {
                try
                {
                    await collection.Indexes.CreateManyAsync(indexModels);
                    Console.WriteLine($"✅ Created {indexModels.Count} indexes for {collectionName} collection");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error creating indexes for {collectionName}: {ex.Message}");
                }
            }
        }
    }
}