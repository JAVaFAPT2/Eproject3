using MongoDB.Bson.Serialization;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Infrastructure.Persistence
{
    /// <summary>
    /// MongoDB class map configuration for domain entities
    /// Configures how MongoDB serializes/deserializes entities with internal parameterless constructors
    /// </summary>
    public static class MongoDbClassMapConfiguration
    {
        private static bool _isConfigured;
        private static readonly object Lock = new object();

        public static void Configure()
        {
            lock (Lock)
            {
                if (_isConfigured)
                    return;

                // Configure Vehicle entity - MongoDB will use internal parameterless constructor via reflection
                BsonClassMap.RegisterClassMap<Vehicle>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                // Configure VehicleModel entity
                BsonClassMap.RegisterClassMap<VehicleModel>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                // Configure User entity
                BsonClassMap.RegisterClassMap<User>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                // Configure Role entity
                BsonClassMap.RegisterClassMap<Role>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                // Configure Order entity
                BsonClassMap.RegisterClassMap<Order>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                // Configure ServiceOrder entity
                BsonClassMap.RegisterClassMap<ServiceOrder>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                // Configure PurchaseOrder entity
                BsonClassMap.RegisterClassMap<PurchaseOrder>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                // Configure PurchaseOrderLine entity
                BsonClassMap.RegisterClassMap<PurchaseOrderLine>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                // Configure BillingDocument entity
                BsonClassMap.RegisterClassMap<BillingDocument>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                // Configure VehiclePhoto entity
                BsonClassMap.RegisterClassMap<VehiclePhoto>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                // Configure VehicleSpec entity
                BsonClassMap.RegisterClassMap<VehicleSpec>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                // Configure DocumentOutput entity
                BsonClassMap.RegisterClassMap<DocumentOutput>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });

                _isConfigured = true;
                Console.WriteLine("✅ MongoDB class maps configured successfully");
            }
        }
    }
}

