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
        public IMongoCollection<Employee> Employees => _database.GetCollection<Employee>("employees");
        public IMongoCollection<Role> Roles => _database.GetCollection<Role>("roles");
        public IMongoCollection<VehicleModel> VehicleModels => _database.GetCollection<VehicleModel>("vehicleModels");
        public IMongoCollection<Vehicle> Vehicles => _database.GetCollection<Vehicle>("vehicles");
        public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>("customers");
        public IMongoCollection<SalesOrder> SalesOrders => _database.GetCollection<SalesOrder>("salesOrders");
        public IMongoCollection<ServiceOrder> ServiceOrders => _database.GetCollection<ServiceOrder>("serviceOrders");
        public IMongoCollection<BillingDocument> BillingDocuments => _database.GetCollection<BillingDocument>("billingDocuments");
        public IMongoCollection<Allotment> Allotments => _database.GetCollection<Allotment>("allotments");
        public IMongoCollection<WaitingList> WaitingLists => _database.GetCollection<WaitingList>("waitingLists");

        // Helper methods
        public IMongoDatabase GetDatabase()
        {
            return _database;
        }

        // IUnitOfWork implementation
        public int SaveChangesAsync()
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
                ("employees", Employees),
                ("roles", Roles),
                ("vehicleModels", VehicleModels),
                ("vehicles", Vehicles),
                ("customers", Customers),
                ("salesOrders", SalesOrders),
                ("serviceOrders", ServiceOrders),
                ("billingDocuments", BillingDocuments),
                ("allotments", Allotments),
                ("waitingLists", WaitingLists)
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
                case "employees":
                    // Authentication indexes (highest priority)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("employeeId"),
                        new CreateIndexOptions { Unique = true, Name = "EmployeeId_Unique" }
                    ));

                    // Query optimization indexes
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("role").Ascending("isDeleted"),
                        new CreateIndexOptions { Name = "Role_IsDeleted" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("status").Ascending("isDeleted"),
                        new CreateIndexOptions { Name = "Status_IsDeleted" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("createdAt"),
                        new CreateIndexOptions { Name = "CreatedAt" }
                    ));
                    break;

                case "vehicles":
                    // Critical business indexes (vehicleId is primary identifier)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("vehicleId"),
                        new CreateIndexOptions { Unique = true, Name = "VehicleId_Unique" }
                    ));

                    // Search and filter indexes (most frequent queries)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("status").Ascending("isDeleted"),
                        new CreateIndexOptions { Name = "Status_IsDeleted" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("modelNumber").Ascending("status"),
                        new CreateIndexOptions { Name = "ModelNumber_Status" }
                    ));

                    // Price-based queries (frequent for sales)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("purchasePrice"),
                        new CreateIndexOptions { Name = "PurchasePrice" }
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

                case "vehicleModels":
                    // Unique index for model number
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("modelNumber"),
                        new CreateIndexOptions { Unique = true, Name = "ModelNumber_Unique" }
                    ));

                    // Query optimization
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("brand").Ascending("isDeleted"),
                        new CreateIndexOptions { Name = "Brand_IsDeleted" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("basePrice"),
                        new CreateIndexOptions { Name = "BasePrice" }
                    ));
                    break;

                case "customers":
                    // Contact indexes (business critical)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("customerId"),
                        new CreateIndexOptions { Unique = true, Name = "CustomerId_Unique" }
                    ));
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

                case "salesOrders":
                    // Business critical indexes (customer queries are most frequent)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("salesOrderId"),
                        new CreateIndexOptions { Unique = true, Name = "SalesOrderId_Unique" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("customerId").Ascending("status"),
                        new CreateIndexOptions { Name = "CustomerId_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("vehicleId").Ascending("status"),
                        new CreateIndexOptions { Name = "VehicleId_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("employeeId").Ascending("status"),
                        new CreateIndexOptions { Name = "EmployeeId_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("status").Ascending("orderDate"),
                        new CreateIndexOptions { Name = "Status_OrderDate" }
                    ));

                    // Date-based analytics (frequent reporting queries)
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("orderDate").Descending("salePrice"),
                        new CreateIndexOptions { Name = "OrderDate_Desc_SalePrice" }
                    ));

                    // Audit trail
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("isDeleted").Ascending("createdAt"),
                        new CreateIndexOptions { Name = "IsDeleted_CreatedAt" }
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
                        Builders<T>.IndexKeys.Ascending("serviceOrderId"),
                        new CreateIndexOptions { Unique = true, Name = "ServiceOrderId_Unique" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("salesOrderId").Ascending("status"),
                        new CreateIndexOptions { Name = "SalesOrderId_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("employeeId").Ascending("status"),
                        new CreateIndexOptions { Name = "EmployeeId_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("serviceDate").Descending("cost"),
                        new CreateIndexOptions { Name = "ServiceDate_Desc_Cost" }
                    ));
                    break;

                case "billingDocuments":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("billId"),
                        new CreateIndexOptions { Unique = true, Name = "BillId_Unique" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("salesOrderId").Ascending("billDate"),
                        new CreateIndexOptions { Name = "SalesOrderId_BillDate" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("employeeId").Ascending("billDate"),
                        new CreateIndexOptions { Name = "EmployeeId_BillDate" }
                    ));
                    break;

                case "allotments":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("allotmentId"),
                        new CreateIndexOptions { Unique = true, Name = "AllotmentId_Unique" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("vehicleId").Ascending("customerId"),
                        new CreateIndexOptions { Name = "VehicleId_CustomerId" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("employeeId").Ascending("allotmentDate"),
                        new CreateIndexOptions { Name = "EmployeeId_AllotmentDate" }
                    ));
                    break;

                case "waitingLists":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("waitId"),
                        new CreateIndexOptions { Unique = true, Name = "WaitId_Unique" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("customerId").Ascending("modelNumber"),
                        new CreateIndexOptions { Name = "CustomerId_ModelNumber" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("status").Ascending("requestDate"),
                        new CreateIndexOptions { Name = "Status_RequestDate" }
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