using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// VehicleRegistration entity representing vehicle registration data
    /// Manages vehicle registration information and documentation
    /// </summary>
    public class VehicleRegistration : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("vehicleId")]
        [BsonRequired]
        public string VehicleId { get; set; } = string.Empty;

        [BsonElement("vin")]
        [BsonRequired]
        public string VIN { get; set; } = string.Empty;

        [BsonElement("registrationNumber")]
        [BsonRequired]
        public string RegistrationNumber { get; set; } = string.Empty;

        [BsonElement("registrationDate")]
        [BsonRequired]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [BsonElement("expiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [BsonElement("registrationAuthority")]
        [BsonRequired]
        public string RegistrationAuthority { get; set; } = string.Empty;

        [BsonElement("registrationState")]
        [BsonRequired]
        public string RegistrationState { get; set; } = string.Empty;

        [BsonElement("registrationCity")]
        [BsonRequired]
        public string RegistrationCity { get; set; } = string.Empty;

        [BsonElement("ownerName")]
        [BsonRequired]
        public string OwnerName { get; set; } = string.Empty;

        [BsonElement("ownerAddress")]
        [BsonRequired]
        public string OwnerAddress { get; set; } = string.Empty;

        [BsonElement("ownerPhone")]
        public string? OwnerPhone { get; set; }

        [BsonElement("ownerEmail")]
        public string? OwnerEmail { get; set; }

        [BsonElement("vehicleType")]
        [BsonRequired]
        public string VehicleType { get; set; } = string.Empty; // Car, SUV, Truck, etc.

        [BsonElement("fuelType")]
        [BsonRequired]
        public string FuelType { get; set; } = string.Empty; // Petrol, Diesel, Electric, Hybrid

        [BsonElement("engineNumber")]
        public string? EngineNumber { get; set; }

        [BsonElement("chassisNumber")]
        public string? ChassisNumber { get; set; }

        [BsonElement("manufacturingYear")]
        [BsonRequired]
        public int ManufacturingYear { get; set; }

        [BsonElement("manufacturingMonth")]
        public int? ManufacturingMonth { get; set; }

        [BsonElement("seatingCapacity")]
        public int? SeatingCapacity { get; set; }

        [BsonElement("grossWeight")]
        public decimal? GrossWeight { get; set; }

        [BsonElement("unladenWeight")]
        public decimal? UnladenWeight { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "Active"; // Active, Expired, Suspended, Cancelled

        [BsonElement("isTransferable")]
        public bool IsTransferable { get; set; } = true;

        [BsonElement("previousOwner")]
        public string? PreviousOwner { get; set; }

        [BsonElement("transferDate")]
        public DateTime? TransferDate { get; set; }

        [BsonElement("insuranceNumber")]
        public string? InsuranceNumber { get; set; }

        [BsonElement("insuranceExpiry")]
        public DateTime? InsuranceExpiry { get; set; }

        [BsonElement("pollutionCertificateNumber")]
        public string? PollutionCertificateNumber { get; set; }

        [BsonElement("pollutionCertificateExpiry")]
        public DateTime? PollutionCertificateExpiry { get; set; }

        [BsonElement("fitnessCertificateNumber")]
        public string? FitnessCertificateNumber { get; set; }

        [BsonElement("fitnessCertificateExpiry")]
        public DateTime? FitnessCertificateExpiry { get; set; }

        [BsonElement("notes")]
        public string? Notes { get; set; }

        [BsonElement("createdBy")]
        [BsonRequired]
        public string CreatedBy { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Private constructor for MongoDB
        private VehicleRegistration() { }

        public VehicleRegistration(
            string vehicleId,
            string vin,
            string registrationNumber,
            DateTime registrationDate,
            string registrationAuthority,
            string registrationState,
            string registrationCity,
            string ownerName,
            string ownerAddress,
            string? ownerPhone = null,
            string? ownerEmail = null,
            string vehicleType = "Car",
            string fuelType = "Petrol",
            string? engineNumber = null,
            string? chassisNumber = null,
            int manufacturingYear = 2024,
            int? manufacturingMonth = null,
            int? seatingCapacity = null,
            decimal? grossWeight = null,
            decimal? unladenWeight = null,
            DateTime? expiryDate = null,
            string createdBy = "")
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be null or empty", nameof(vehicleId));

            if (string.IsNullOrWhiteSpace(vin))
                throw new ArgumentException("VIN cannot be null or empty", nameof(vin));

            if (string.IsNullOrWhiteSpace(registrationNumber))
                throw new ArgumentException("Registration number cannot be null or empty", nameof(registrationNumber));

            if (string.IsNullOrWhiteSpace(registrationAuthority))
                throw new ArgumentException("Registration authority cannot be null or empty", nameof(registrationAuthority));

            if (string.IsNullOrWhiteSpace(registrationState))
                throw new ArgumentException("Registration state cannot be null or empty", nameof(registrationState));

            if (string.IsNullOrWhiteSpace(registrationCity))
                throw new ArgumentException("Registration city cannot be null or empty", nameof(registrationCity));

            if (string.IsNullOrWhiteSpace(ownerName))
                throw new ArgumentException("Owner name cannot be null or empty", nameof(ownerName));

            if (string.IsNullOrWhiteSpace(ownerAddress))
                throw new ArgumentException("Owner address cannot be null or empty", nameof(ownerAddress));

            VehicleId = vehicleId;
            VIN = vin;
            RegistrationNumber = registrationNumber;
            RegistrationDate = registrationDate;
            ExpiryDate = expiryDate;
            RegistrationAuthority = registrationAuthority;
            RegistrationState = registrationState;
            RegistrationCity = registrationCity;
            OwnerName = ownerName;
            OwnerAddress = ownerAddress;
            OwnerPhone = ownerPhone;
            OwnerEmail = ownerEmail;
            VehicleType = vehicleType;
            FuelType = fuelType;
            EngineNumber = engineNumber;
            ChassisNumber = chassisNumber;
            ManufacturingYear = manufacturingYear;
            ManufacturingMonth = manufacturingMonth;
            SeatingCapacity = seatingCapacity;
            GrossWeight = grossWeight;
            UnladenWeight = unladenWeight;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain Methods
        public void ActivateRegistration()
        {
            Status = "Active";
            UpdatedAt = DateTime.UtcNow;
        }

        public void SuspendRegistration(string reason)
        {
            Status = "Suspended";
            Notes = reason;
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelRegistration(string reason)
        {
            Status = "Cancelled";
            Notes = reason;
            UpdatedAt = DateTime.UtcNow;
        }

        public void TransferOwnership(string newOwnerName, string newOwnerAddress)
        {
            PreviousOwner = OwnerName;
            OwnerName = newOwnerName;
            OwnerAddress = newOwnerAddress;
            TransferDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsExpired()
        {
            return ExpiryDate.HasValue && ExpiryDate.Value < DateTime.UtcNow;
        }

        public bool IsActive()
        {
            return Status == "Active" && !IsExpired();
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
