using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Vehicle entity representing physical vehicles in inventory
    /// VehicleId is the primary key (_id in MongoDB)
    /// </summary>
    public class Vehicle
    {
        [BsonId]
        public string VehicleId { get; private set; } = string.Empty;

        [BsonElement("modelNumber")]
        [BsonRequired]
        public string ModelNumber { get; private set; } = string.Empty;

        [BsonElement("externalNumber")]
        public string? ExternalNumber { get; private set; }

        [BsonElement("registrationData")]
        public BsonDocument? RegistrationData { get; private set; }

        [BsonElement("status")]
        public VehicleStatus Status { get; private set; } = VehicleStatus.InStock;

        [BsonElement("purchasePrice")]
        public decimal PurchasePrice { get; private set; }

        [BsonElement("licensePlate")]
        public string? LicensePlate { get; private set; }


        // Parameterless constructor for MongoDB deserialization
        public Vehicle() { }

        public Vehicle(string vehicleId, string modelNumber, decimal purchasePrice, 
            string? externalNumber = null)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be null or empty", nameof(vehicleId));

            if (string.IsNullOrWhiteSpace(modelNumber))
                throw new ArgumentException("Model number cannot be null or empty", nameof(modelNumber));

            if (purchasePrice < 0)
                throw new ArgumentException("Purchase price cannot be negative", nameof(purchasePrice));

            VehicleId = vehicleId;
            ModelNumber = modelNumber;
            PurchasePrice = purchasePrice;
            ExternalNumber = externalNumber;
        }

        // Domain methods
        public void UpdateStatus(VehicleStatus status)
        {
            Status = status;
        }

        public void UpdateRegistrationData(BsonDocument registrationData)
        {
            RegistrationData = registrationData;
        }

        public void UpdateExternalNumber(string externalNumber)
        {
            ExternalNumber = externalNumber;
        }

        public void Reserve()
        {
            if (Status != VehicleStatus.InStock)
                throw new InvalidOperationException("Only vehicles in stock can be reserved");

            Status = VehicleStatus.Reserved;
        }

        public void Sell()
        {
            if (Status != VehicleStatus.Reserved && Status != VehicleStatus.InStock)
                throw new InvalidOperationException("Only reserved or in-stock vehicles can be sold");

            Status = VehicleStatus.Sold;
        }

        public void SetLicensePlate(string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                throw new ArgumentException("License plate cannot be empty", nameof(licensePlate));
            LicensePlate = licensePlate;
        }

        public void SendToService()
        {
            Status = VehicleStatus.InService;
        }

        public void ReturnToStock()
        {
            if (Status == VehicleStatus.Sold)
                throw new InvalidOperationException("Sold vehicles cannot be returned to stock");

            Status = VehicleStatus.InStock;
        }

        // Computed properties
        public bool IsAvailable => Status == VehicleStatus.InStock;
        public bool IsSold => Status == VehicleStatus.Sold;
        public bool IsReserved => Status == VehicleStatus.Reserved;
    }
}
