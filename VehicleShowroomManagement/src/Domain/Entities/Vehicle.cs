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
        public VehicleStatus Status { get; private set; } = VehicleStatus.Available;

        [BsonElement("purchasePrice")]
        public decimal PurchasePrice { get; private set; }

        [BsonElement("licensePlate")]
        public string? LicensePlate { get; private set; }

        [BsonElement("vin")]
        public string? Vin { get; private set; }


        // Parameterless constructor for MongoDB deserialization
        public Vehicle() { }

        public Vehicle(string vehicleId, string modelNumber, decimal purchasePrice, 
            string? externalNumber = null, string? vin = null)
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
            Vin = vin;
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

        public void SetVin(string? vin)
        {
            Vin = vin;
        }

        public void Reserve()
        {
            if (Status != VehicleStatus.Available)
                throw new InvalidOperationException("Only vehicles in stock can be reserved");

            Status = VehicleStatus.Reserved;
        }

        public void Sell()
        {
            if (Status != VehicleStatus.Reserved && Status != VehicleStatus.Available)
                throw new InvalidOperationException("Only reserved or in-stock vehicles can be sold");

            Status = VehicleStatus.Sold;
        }

        public void SetLicensePlate(string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                throw new ArgumentException("License plate cannot be empty", nameof(licensePlate));
            LicensePlate = licensePlate;
        }


        public void ReturnToStock()
        {
            if (Status == VehicleStatus.Sold)
                throw new InvalidOperationException("Sold vehicles cannot be returned to stock");

            Status = VehicleStatus.Available;
        }

        // Computed properties
        public bool IsAvailable => Status == VehicleStatus.Available;
        public bool IsSold => Status == VehicleStatus.Sold;
        public bool IsReserved => Status == VehicleStatus.Reserved;
    }
}
