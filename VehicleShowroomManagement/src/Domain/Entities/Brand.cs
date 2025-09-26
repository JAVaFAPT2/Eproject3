using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Brand entity representing vehicle manufacturers
    /// </summary>
    public class Brand : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("brandName")]
        [BsonRequired]
        public string BrandName { get; set; } = string.Empty;

        [BsonElement("country")]
        public string? Country { get; set; }

        [BsonElement("logoUrl")]
        public string? LogoUrl { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // References to models
        [BsonElement("modelIds")]
        public List<string> ModelIds { get; set; } = new List<string>();

        // Domain Methods
        public void UpdateBrand(string brandName, string? country, string? logoUrl)
        {
            BrandName = brandName;
            Country = country;
            LogoUrl = logoUrl;
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