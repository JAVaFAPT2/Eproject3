using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Vehicle entity representing vehicles in the showroom inventory
    /// Core entity for the vehicle showroom management system
    /// </summary>
    public class Vehicle : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("vin")]
        [BsonRequired]
        public string VIN { get; set; } = string.Empty; // Vehicle Identification Number

        [BsonElement("modelId")]
        [BsonRequired]
        public string ModelId { get; set; } = string.Empty;

        [BsonElement("color")]
        [BsonRequired]
        public string Color { get; set; } = string.Empty;

        [BsonElement("year")]
        [BsonRequired]
        public int Year { get; set; }

        [BsonElement("price")]
        [BsonRequired]
        public decimal Price { get; set; }

        [BsonElement("mileage")]
        public int Mileage { get; set; } = 0;

        [BsonElement("status")]
        public string Status { get; set; } = "Available"; // Available, Sold, InService, Reserved

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Embedded model information
        [BsonElement("model")]
        public ModelInfo? Model { get; set; }

        // Embedded images
        [BsonElement("images")]
        public List<VehicleImage> VehicleImages { get; set; } = new List<VehicleImage>();

        // References to other documents
        [BsonElement("salesOrderItemIds")]
        public List<string> SalesOrderItemIds { get; set; } = new List<string>();

        // Domain Methods
        public void UpdateVehicleInfo(string color, int year, decimal price, int mileage)
        {
            Color = color;
            Year = year;
            Price = price;
            Mileage = mileage;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsSold()
        {
            Status = "Sold";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsAvailable()
        {
            Status = "Available";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsInService()
        {
            Status = "InService";
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reserve()
        {
            Status = "Reserved";
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsAvailableForSale()
        {
            return Status == "Available";
        }

        public bool CanBeServiced()
        {
            return Status != "InService";
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

    /// <summary>
    /// Embedded model information within Vehicle document
    /// </summary>
    public class ModelInfo
    {
        [BsonElement("modelId")]
        [BsonRequired]
        public string ModelId { get; set; } = string.Empty;

        [BsonElement("modelName")]
        [BsonRequired]
        public string ModelName { get; set; } = string.Empty;

        [BsonElement("brandId")]
        [BsonRequired]
        public string BrandId { get; set; } = string.Empty;

        [BsonElement("brand")]
        public BrandInfo? Brand { get; set; }

        [BsonElement("engineType")]
        public string? EngineType { get; set; }

        [BsonElement("transmission")]
        public string? Transmission { get; set; }

        [BsonElement("fuelType")]
        public string? FuelType { get; set; }

        [BsonElement("seatingCapacity")]
        public int? SeatingCapacity { get; set; }
    }

    /// <summary>
    /// Embedded brand information within Model document
    /// </summary>
    public class BrandInfo
    {
        [BsonElement("brandId")]
        [BsonRequired]
        public string BrandId { get; set; } = string.Empty;

        [BsonElement("brandName")]
        [BsonRequired]
        public string BrandName { get; set; } = string.Empty;

        [BsonElement("country")]
        public string? Country { get; set; }

        [BsonElement("logoUrl")]
        public string? LogoUrl { get; set; }
    }
}