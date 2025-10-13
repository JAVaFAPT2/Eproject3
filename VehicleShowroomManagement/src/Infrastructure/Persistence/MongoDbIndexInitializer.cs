using MongoDB.Driver;

namespace VehicleShowroomManagement.Infrastructure.Persistence
{
    /// <summary>
    /// Service to initialize MongoDB indexes for optimal performance
    /// </summary>
    public class MongoDbIndexInitializer(VehicleShowroomDbContext context)
    {
        private readonly IMongoDatabase _database = context.GetDatabase();

        /// <summary>
        /// Creates all necessary indexes for the application
        /// Skips indexes that already exist to prevent conflicts
        /// </summary>
        public async Task InitializeIndexesAsync()
        {
            Console.WriteLine("🔍 Initializing additional performance indexes...");
            
            try
            {
                await CreateUserIndexesAsync();
                await CreateVehicleIndexesAsync();
                await CreateVehicleModelIndexesAsync();
                await CreateOrderIndexesAsync();
                await CreateServiceOrderIndexesAsync();
                await CreateBillingDocumentIndexesAsync();
                await CreatePurchaseOrderIndexesAsync();
                
                Console.WriteLine("✅ Additional performance indexes initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Warning during index initialization: {ex.Message}");
                Console.WriteLine("   Application will continue with existing indexes.");
            }
        }

        private async Task CreateUserIndexesAsync()
        {
            var collection = _database.GetCollection<MongoDB.Bson.BsonDocument>("USER");

            try
            {
                // Check existing indexes
                var existingIndexes = await (await collection.Indexes.ListAsync()).ToListAsync();
                var existingIndexNames = existingIndexes.Select(idx => idx["name"].AsString).ToHashSet();

                // Only create indexes that don't exist
                var indexesToCreate = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>();

                if (!existingIndexNames.Any(name => name.Contains("username")))
                {
                    var usernameIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("username");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        usernameIndex,
                        new CreateIndexOptions { Unique = true, Name = "idx_user_username" }));
                }

                if (!existingIndexNames.Any(name => name.Contains("email")))
                {
                    var emailIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("email");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        emailIndex,
                        new CreateIndexOptions { Unique = true, Name = "idx_user_email" }));
                }

                if (!existingIndexNames.Contains("idx_user_roleId"))
                {
                    var roleIdIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("roleId");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        roleIdIndex,
                        new CreateIndexOptions { Name = "idx_user_roleId" }));
                }

                if (!existingIndexNames.Contains("idx_user_status"))
                {
                    var statusIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("status");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        statusIndex,
                        new CreateIndexOptions { Name = "idx_user_status" }));
                }

                if (!existingIndexNames.Contains("idx_user_deletedAt"))
                {
                    var deletedAtIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("deletedAt");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        deletedAtIndex,
                        new CreateIndexOptions { Name = "idx_user_deletedAt", Sparse = true }));
                }

                if (indexesToCreate.Any())
                {
                    await collection.Indexes.CreateManyAsync(indexesToCreate);
                    Console.WriteLine($"✅ Created {indexesToCreate.Count} new indexes for USER collection");
                }
                else
                {
                    Console.WriteLine("ℹ️ All USER indexes already exist, skipping creation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Note: Some USER indexes may already exist - {ex.Message}");
            }
        }

        private async Task CreateVehicleIndexesAsync()
        {
            var collection = _database.GetCollection<MongoDB.Bson.BsonDocument>("VEHICLE");

            try
            {
                var existingIndexes = await (await collection.Indexes.ListAsync()).ToListAsync();
                var existingIndexNames = existingIndexes.Select(idx => idx["name"].AsString).ToHashSet();
                var indexesToCreate = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>();

                if (!existingIndexNames.Contains("idx_vehicle_modelNumber"))
                {
                    var modelNumberIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("modelNumber");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        modelNumberIndex,
                        new CreateIndexOptions { Name = "idx_vehicle_modelNumber" }));
                }

                if (!existingIndexNames.Contains("idx_vehicle_status"))
                {
                    var statusIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("status");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        statusIndex,
                        new CreateIndexOptions { Name = "idx_vehicle_status" }));
                }

                if (!existingIndexNames.Contains("idx_vehicle_purchasePrice"))
                {
                    var priceIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("purchasePrice");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        priceIndex,
                        new CreateIndexOptions { Name = "idx_vehicle_purchasePrice" }));
                }

                if (!existingIndexNames.Contains("idx_vehicle_status_modelNumber"))
                {
                    var compoundIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys
                        .Ascending("status")
                        .Ascending("modelNumber");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        compoundIndex,
                        new CreateIndexOptions { Name = "idx_vehicle_status_modelNumber" }));
                }

                if (indexesToCreate.Any())
                {
                    await collection.Indexes.CreateManyAsync(indexesToCreate);
                    Console.WriteLine($"✅ Created {indexesToCreate.Count} new indexes for VEHICLE collection");
                }
                else
                {
                    Console.WriteLine("ℹ️ All VEHICLE indexes already exist, skipping creation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Note: Some VEHICLE indexes may already exist - {ex.Message}");
            }
        }

        private async Task CreateVehicleModelIndexesAsync()
        {
            var collection = _database.GetCollection<MongoDB.Bson.BsonDocument>("VEHICLE_MODEL");

            try
            {
                var existingIndexes = await (await collection.Indexes.ListAsync()).ToListAsync();
                var existingIndexNames = existingIndexes.Select(idx => idx["name"].AsString).ToHashSet();
                var indexesToCreate = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>();

                if (!existingIndexNames.Contains("idx_vehiclemodel_brand"))
                {
                    var brandIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("brand");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        brandIndex,
                        new CreateIndexOptions { Name = "idx_vehiclemodel_brand" }));
                }

                if (!existingIndexNames.Contains("idx_vehiclemodel_price"))
                {
                    var priceIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("price");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        priceIndex,
                        new CreateIndexOptions { Name = "idx_vehiclemodel_price" }));
                }

                if (indexesToCreate.Any())
                {
                    await collection.Indexes.CreateManyAsync(indexesToCreate);
                    Console.WriteLine($"✅ Created {indexesToCreate.Count} new indexes for VEHICLE_MODEL collection");
                }
                else
                {
                    Console.WriteLine("ℹ️ All VEHICLE_MODEL indexes already exist, skipping creation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Note: Some VEHICLE_MODEL indexes may already exist - {ex.Message}");
            }
        }

        private async Task CreateOrderIndexesAsync()
        {
            var collection = _database.GetCollection<MongoDB.Bson.BsonDocument>("ORDER");
            
            try
            {
                var existingIndexes = await (await collection.Indexes.ListAsync()).ToListAsync();
                var existingIndexNames = existingIndexes.Select(idx => idx["name"].AsString).ToHashSet();
                var indexesToCreate = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>();

                if (!existingIndexNames.Contains("idx_order_customerId"))
                {
                    var customerIdIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("customerId");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        customerIdIndex,
                        new CreateIndexOptions { Name = "idx_order_customerId" }));
                }

                if (!existingIndexNames.Contains("idx_order_dealerId"))
                {
                    var dealerIdIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("dealerId");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        dealerIdIndex,
                        new CreateIndexOptions { Name = "idx_order_dealerId" }));
                }

                if (!existingIndexNames.Contains("idx_order_status"))
                {
                    var statusIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("status");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        statusIndex,
                        new CreateIndexOptions { Name = "idx_order_status" }));
                }

                if (!existingIndexNames.Contains("idx_order_orderDate"))
                {
                    var orderDateIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("orderDate");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        orderDateIndex,
                        new CreateIndexOptions { Name = "idx_order_orderDate" }));
                }

                if (!existingIndexNames.Contains("idx_order_vehicleId"))
                {
                    var vehicleIdIndex = Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("vehicleId");
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        vehicleIdIndex,
                        new CreateIndexOptions { Name = "idx_order_vehicleId", Sparse = true }));
                }

                if (indexesToCreate.Any())
                {
                    await collection.Indexes.CreateManyAsync(indexesToCreate);
                    Console.WriteLine($"✅ Created {indexesToCreate.Count} new indexes for ORDER collection");
                }
                else
                {
                    Console.WriteLine("ℹ️ All ORDER indexes already exist, skipping creation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Note: Some ORDER indexes may already exist - {ex.Message}");
            }
        }

        private async Task CreateServiceOrderIndexesAsync()
        {
            try
            {
                var collection = _database.GetCollection<MongoDB.Bson.BsonDocument>("SERVICE_ORDER");
                var existingIndexes = await (await collection.Indexes.ListAsync()).ToListAsync();
                var existingIndexNames = existingIndexes.Select(idx => idx["name"].AsString).ToHashSet();
                var indexesToCreate = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>();

                if (!existingIndexNames.Contains("idx_serviceorder_orderId") && !existingIndexNames.Any(n => n.Contains("OrderId")))
                {
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("orderId"),
                        new CreateIndexOptions { Name = "idx_serviceorder_orderId" }));
                }

                if (!existingIndexNames.Contains("idx_serviceorder_status"))
                {
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("status"),
                        new CreateIndexOptions { Name = "idx_serviceorder_status" }));
                }

                if (!existingIndexNames.Contains("idx_serviceorder_createdBy") && !existingIndexNames.Any(n => n.Contains("CreatedBy")))
                {
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("createdBy"),
                        new CreateIndexOptions { Name = "idx_serviceorder_createdBy" }));
                }

                if (indexesToCreate.Any())
                {
                    await collection.Indexes.CreateManyAsync(indexesToCreate);
                    Console.WriteLine($"✅ Created {indexesToCreate.Count} new indexes for SERVICE_ORDER collection");
                }
                else
                {
                    Console.WriteLine("ℹ️ All SERVICE_ORDER indexes already exist, skipping creation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Note: Some SERVICE_ORDER indexes may already exist - {ex.Message}");
            }
        }

        private async Task CreateBillingDocumentIndexesAsync()
        {
            try
            {
                var collection = _database.GetCollection<MongoDB.Bson.BsonDocument>("BILLING_DOCUMENT");
                var existingIndexes = await (await collection.Indexes.ListAsync()).ToListAsync();
                var existingIndexNames = existingIndexes.Select(idx => idx["name"].AsString).ToHashSet();
                var indexesToCreate = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>();

                if (!existingIndexNames.Contains("idx_billingdoc_orderId") && !existingIndexNames.Any(n => n.Contains("OrderId")))
                {
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("orderId"),
                        new CreateIndexOptions { Name = "idx_billingdoc_orderId" }));
                }

                if (!existingIndexNames.Contains("idx_billingdoc_status") && !existingIndexNames.Any(n => n.Contains("Status") && n.Contains("BillDate")))
                {
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("status"),
                        new CreateIndexOptions { Name = "idx_billingdoc_status" }));
                }

                if (!existingIndexNames.Contains("idx_billingdoc_createdBy") && !existingIndexNames.Any(n => n.Contains("CreatedBy") && n.Contains("BillDate")))
                {
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("createdBy"),
                        new CreateIndexOptions { Name = "idx_billingdoc_createdBy" }));
                }

                if (!existingIndexNames.Contains("idx_billingdoc_billDate"))
                {
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("billDate"),
                        new CreateIndexOptions { Name = "idx_billingdoc_billDate" }));
                }

                if (indexesToCreate.Any())
                {
                    await collection.Indexes.CreateManyAsync(indexesToCreate);
                    Console.WriteLine($"✅ Created {indexesToCreate.Count} new indexes for BILLING_DOCUMENT collection");
                }
                else
                {
                    Console.WriteLine("ℹ️ All BILLING_DOCUMENT indexes already exist, skipping creation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Note: Some BILLING_DOCUMENT indexes may already exist - {ex.Message}");
            }
        }

        private async Task CreatePurchaseOrderIndexesAsync()
        {
            try
            {
                var collection = _database.GetCollection<MongoDB.Bson.BsonDocument>("PURCHASE_ORDER");
                var existingIndexes = await (await collection.Indexes.ListAsync()).ToListAsync();
                var existingIndexNames = existingIndexes.Select(idx => idx["name"].AsString).ToHashSet();
                var indexesToCreate = new List<CreateIndexModel<MongoDB.Bson.BsonDocument>>();

                if (!existingIndexNames.Contains("idx_purchaseorder_createdBy") && !existingIndexNames.Any(n => n.Contains("CreatedBy")))
                {
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("createdBy"),
                        new CreateIndexOptions { Name = "idx_purchaseorder_createdBy" }));
                }

                if (!existingIndexNames.Contains("idx_purchaseorder_status") && !existingIndexNames.Any(n => n.Contains("Status")))
                {
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Ascending("status"),
                        new CreateIndexOptions { Name = "idx_purchaseorder_status" }));
                }

                if (!existingIndexNames.Contains("idx_purchaseorder_orderDate") && !existingIndexNames.Any(n => n.Contains("OrderDate")))
                {
                    indexesToCreate.Add(new CreateIndexModel<MongoDB.Bson.BsonDocument>(
                        Builders<MongoDB.Bson.BsonDocument>.IndexKeys.Descending("orderDate"),
                        new CreateIndexOptions { Name = "idx_purchaseorder_orderDate" }));
                }

                if (indexesToCreate.Any())
                {
                    await collection.Indexes.CreateManyAsync(indexesToCreate);
                    Console.WriteLine($"✅ Created {indexesToCreate.Count} new indexes for PURCHASE_ORDER collection");
                }
                else
                {
                    Console.WriteLine("ℹ️ All PURCHASE_ORDER indexes already exist, skipping creation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Note: Some PURCHASE_ORDER indexes may already exist - {ex.Message}");
            }
        }
    }
}

