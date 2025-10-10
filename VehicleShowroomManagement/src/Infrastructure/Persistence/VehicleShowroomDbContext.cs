using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Infrastructure.Persistence
{
    /// <summary>
    /// MongoDB database context for Vehicle Showroom Management System
    /// Implements Unit of Work pattern
    /// </summary>
    public class VehicleShowroomDbContext : IUnitOfWork
    {
        private readonly IMongoDatabase _database;
        private IClientSessionHandle? _session;

        public VehicleShowroomDbContext(IMongoDatabase database)
        {
            _database = database;
        }

        // MongoDB Collections with uppercase naming
        public IMongoCollection<Role> Roles => _database.GetCollection<Role>("ROLE");
        public IMongoCollection<User> Users => _database.GetCollection<User>("USER");
        public IMongoCollection<VehicleModel> VehicleModels => _database.GetCollection<VehicleModel>("VEHICLE_MODEL");
        public IMongoCollection<Vehicle> Vehicles => _database.GetCollection<Vehicle>("VEHICLE");
        public IMongoCollection<VehiclePhoto> VehiclePhotos => _database.GetCollection<VehiclePhoto>("VEHICLE_PHOTO");
        public IMongoCollection<VehicleSpec> VehicleSpecs => _database.GetCollection<VehicleSpec>("VEHICLE_SPEC");
        public IMongoCollection<PurchaseOrder> PurchaseOrders => _database.GetCollection<PurchaseOrder>("PURCHASE_ORDER");
        public IMongoCollection<PurchaseOrderLine> PurchaseOrderLines => _database.GetCollection<PurchaseOrderLine>("PURCHASE_ORDER_LINE");
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("ORDER");
        public IMongoCollection<ServiceOrder> ServiceOrders => _database.GetCollection<ServiceOrder>("SERVICE_ORDER");
        public IMongoCollection<BillingDocument> BillingDocuments => _database.GetCollection<BillingDocument>("BILLING_DOCUMENT");
        public IMongoCollection<DocumentOutput> DocumentOutputs => _database.GetCollection<DocumentOutput>("DOCUMENT_OUTPUT");

        // Helper methods
        public IMongoDatabase GetDatabase()
        {
            return _database;
        }

        // IUnitOfWork implementation
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
                var client = _database.Client;
                _session = await client.StartSessionAsync(cancellationToken: cancellationToken);
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
            _session?.Dispose();
        }

        // Initialize collections with indexes
        public async Task InitializeCollectionsAsync()
        {
            var collectionNames = new (string, dynamic)[]
            {
                ("ROLE", Roles),
                ("USER", Users),
                ("VEHICLE_MODEL", VehicleModels),
                ("VEHICLE", Vehicles),
                ("VEHICLE_PHOTO", VehiclePhotos),
                ("VEHICLE_SPEC", VehicleSpecs),
                ("PURCHASE_ORDER", PurchaseOrders),
                ("PURCHASE_ORDER_LINE", PurchaseOrderLines),
                ("ORDER", Orders),
                ("SERVICE_ORDER", ServiceOrders),
                ("BILLING_DOCUMENT", BillingDocuments),
                ("DOCUMENT_OUTPUT", DocumentOutputs)
            };

            foreach (var (name, collection) in collectionNames)
            {
                await CreateIndexesAsync(collection, name);
            }
        }

        private async Task CreateIndexesAsync<T>(IMongoCollection<T> collection, string collectionName)
        {
            var indexModels = new List<CreateIndexModel<T>>();

            switch (collectionName)
            {
                case "ROLE":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("name"),
                        new CreateIndexOptions { Unique = true, Name = "Name_Unique" }
                    ));
                    break;

                case "USER":
                    // Unique indexes for authentication
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("username"),
                        new CreateIndexOptions { Unique = true, Name = "Username_Unique" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("email"),
                        new CreateIndexOptions { Unique = true, Name = "Email_Unique" }
                    ));

                    // Query optimization indexes
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("roleId").Ascending("status"),
                        new CreateIndexOptions { Name = "RoleId_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("deletedAt"),
                        new CreateIndexOptions { Name = "DeletedAt" }
                    ));
                    break;

                case "VEHICLE_MODEL":
                    // Unique index for model number
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("modelNumber"),
                        new CreateIndexOptions { Unique = true, Name = "ModelNumber_Unique" }
                    ));

                    // Query optimization
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("brand").Ascending("price"),
                        new CreateIndexOptions { Name = "Brand_Price" }
                    ));
                    break;

                case "VEHICLE":
                    // Unique index for vehicle ID
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("vehicleId"),
                        new CreateIndexOptions { Unique = true, Name = "VehicleId_Unique" }
                    ));

                    // Search and filter indexes
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("modelNumber").Ascending("status"),
                        new CreateIndexOptions { Name = "ModelNumber_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("status").Ascending("receiptDate"),
                        new CreateIndexOptions { Name = "Status_ReceiptDate" }
                    ));
                    break;

                case "VEHICLE_PHOTO":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("vehicleId").Ascending("displayOrder"),
                        new CreateIndexOptions { Name = "VehicleId_DisplayOrder" }
                    ));
                    break;

                case "VEHICLE_SPEC":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("vehicleId").Ascending("groupName").Ascending("displayOrder"),
                        new CreateIndexOptions { Name = "VehicleId_GroupName_DisplayOrder" }
                    ));
                    break;

                case "PURCHASE_ORDER":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("createdBy").Ascending("status"),
                        new CreateIndexOptions { Name = "CreatedBy_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("orderDate").Descending("totalAmount"),
                        new CreateIndexOptions { Name = "OrderDate_Desc_TotalAmount" }
                    ));
                    break;

                case "PURCHASE_ORDER_LINE":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("poId"),
                        new CreateIndexOptions { Name = "POId" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("modelNumber"),
                        new CreateIndexOptions { Name = "ModelNumber" }
                    ));
                    break;

                case "ORDER":
                    // Business critical indexes
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("customerId").Ascending("status"),
                        new CreateIndexOptions { Name = "CustomerId_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("dealerId").Ascending("status"),
                        new CreateIndexOptions { Name = "DealerId_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("vehicleId").Ascending("status"),
                        new CreateIndexOptions { Name = "VehicleId_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("modelNumber").Ascending("status"),
                        new CreateIndexOptions { Name = "ModelNumber_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("status").Ascending("orderDate"),
                        new CreateIndexOptions { Name = "Status_OrderDate" }
                    ));
                    break;

                case "SERVICE_ORDER":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("orderId").Ascending("status"),
                        new CreateIndexOptions { Name = "OrderId_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("createdBy").Ascending("type"),
                        new CreateIndexOptions { Name = "CreatedBy_Type" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("appointmentDate"),
                        new CreateIndexOptions { Name = "AppointmentDate" }
                    ));
                    break;

                case "BILLING_DOCUMENT":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("orderId").Ascending("status"),
                        new CreateIndexOptions { Name = "OrderId_Status" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("createdBy").Ascending("billDate"),
                        new CreateIndexOptions { Name = "CreatedBy_BillDate" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("status").Ascending("billDate"),
                        new CreateIndexOptions { Name = "Status_BillDate" }
                    ));
                    break;

                case "DOCUMENT_OUTPUT":
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("entityType").Ascending("entityId"),
                        new CreateIndexOptions { Name = "EntityType_EntityId" }
                    ));
                    indexModels.Add(new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending("fileType").Ascending("createdAt"),
                        new CreateIndexOptions { Name = "FileType_CreatedAt" }
                    ));
                    break;
            }

            // Create indexes if any
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
