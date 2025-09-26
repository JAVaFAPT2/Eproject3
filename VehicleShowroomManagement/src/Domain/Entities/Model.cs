using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Model entity representing specific vehicle models under a brand
    /// </summary>
    public class Model : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("modelName")]
        [BsonRequired]
        public string ModelName { get; set; } = string.Empty;

        [BsonElement("brandId")]
        [BsonRequired]
        public string BrandId { get; set; } = string.Empty;

        [BsonElement("engineType")]
        public string? EngineType { get; set; }

        [BsonElement("transmission")]
        public string? Transmission { get; set; }

        [BsonElement("fuelType")]
        public string? FuelType { get; set; }

        [BsonElement("seatingCapacity")]
        public int? SeatingCapacity { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Embedded brand information
        [BsonElement("brand")]
        public BrandInfo? Brand { get; set; }

        // References to vehicles
        [BsonElement("vehicleIds")]
        public List<string> VehicleIds { get; set; } = new List<string>();

        // Domain Methods
        public void UpdateModel(string modelName, string? engineType, string? transmission, string? fuelType, int? seatingCapacity)
        {
            ModelName = modelName;
            EngineType = engineType;
            Transmission = transmission;
            FuelType = fuelType;
            SeatingCapacity = seatingCapacity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }
    }

}