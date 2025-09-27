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

        [BsonElement("vehicleId")]
        [BsonRequired]
        public string VehicleId { get; set; } = string.Empty;

        [BsonElement("modelNumber")]
        [BsonRequired]
        public string ModelNumber { get; set; } = string.Empty;

        [BsonElement("externalNumber")]
        public string? ExternalNumber { get; set; }

        [BsonElement("registrationData")]
        public RegistrationData? RegistrationData { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "Available"; // Available, Sold, InService, Reserved

        [BsonElement("purchasePrice")]
        [BsonRequired]
        public decimal PurchasePrice { get; set; }

        [BsonElement("photos")]
        public List<string> Photos { get; set; } = new List<string>();

        [BsonElement("receiptDate")]
        public DateTime ReceiptDate { get; set; } = DateTime.UtcNow;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Domain Methods
        public void UpdateVehicleInfo(string modelNumber, decimal purchasePrice, List<string> photos)
        {
            ModelNumber = modelNumber;
            PurchasePrice = purchasePrice;
            Photos = photos ?? new List<string>();
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateRegistrationData(string vin, string licensePlate, DateTime registrationDate, DateTime expiryDate)
        {
            RegistrationData = new RegistrationData
            {
                VIN = vin,
                LicensePlate = licensePlate,
                RegistrationDate = registrationDate,
                ExpiryDate = expiryDate
            };
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
    /// Registration data embedded in Vehicle entity
    /// </summary>
    public class RegistrationData
    {
        [BsonElement("vin")]
        [BsonRequired]
        public string VIN { get; set; } = string.Empty;

        [BsonElement("licensePlate")]
        [BsonRequired]
        public string LicensePlate { get; set; } = string.Empty;

        [BsonElement("registrationDate")]
        [BsonRequired]
        public DateTime RegistrationDate { get; set; }

        [BsonElement("expiryDate")]
        [BsonRequired]
        public DateTime ExpiryDate { get; set; }
    }
}