using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// VehicleModel entity representing specific vehicle models
    /// </summary>
    public class VehicleModel : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("modelNumber")]
        [BsonRequired]
        public string ModelNumber { get; set; } = string.Empty;

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; } = string.Empty;

        [BsonElement("brand")]
        [BsonRequired]
        public string Brand { get; set; } = string.Empty;

        [BsonElement("basePrice")]
        [BsonRequired]
        public decimal BasePrice { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // References to vehicles
        [BsonElement("vehicleIds")]
        public List<string> VehicleIds { get; set; } = new List<string>();

        // Domain Methods
        public void UpdateModel(string name, string brand, decimal basePrice)
        {
            Name = name;
            Brand = brand;
            BasePrice = basePrice;
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

