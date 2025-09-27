using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Enums;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Vehicle aggregate root representing vehicles in the showroom inventory
    /// </summary>
    public class Vehicle : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("vehicleId")]
        [BsonRequired]
        public string VehicleId { get; private set; } = string.Empty;

        [BsonElement("modelNumber")]
        [BsonRequired]
        public string ModelNumber { get; private set; } = string.Empty;

        [BsonElement("externalNumber")]
        public string? ExternalNumber { get; private set; }

        [BsonElement("vin")]
        public string? Vin { get; private set; }

        [BsonElement("licensePlate")]
        public string? LicensePlate { get; private set; }

        [BsonElement("registrationDate")]
        public DateTime? RegistrationDate { get; private set; }

        [BsonElement("expiryDate")]
        public DateTime? ExpiryDate { get; private set; }

        [BsonElement("status")]
        public VehicleStatus Status { get; private set; } = VehicleStatus.Available;

        [BsonElement("purchasePrice")]
        public decimal PurchasePrice { get; private set; }

        [BsonElement("salePrice")]
        public decimal? SalePrice { get; private set; }

        [BsonElement("photos")]
        public List<string> Photos { get; private set; } = new List<string>();

        [BsonElement("receiptDate")]
        public DateTime ReceiptDate { get; private set; } = DateTime.UtcNow;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Private constructor for MongoDB
        private Vehicle() { }

        public Vehicle(string vehicleId, string modelNumber, decimal purchasePrice, string? externalNumber = null, Vin? vin = null, string? licensePlate = null, DateTime? receiptDate = null)
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
            Vin = vin?.Value;
            LicensePlate = licensePlate;
            ReceiptDate = receiptDate ?? DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain methods
        public void UpdateStatus(VehicleStatus status)
        {
            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePurchasePrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("Purchase price cannot be negative", nameof(price));

            PurchasePrice = price;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetSalePrice(decimal? price)
        {
            if (price.HasValue && price < 0)
                throw new ArgumentException("Sale price cannot be negative", nameof(price));

            SalePrice = price;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateRegistrationInfo(Vin vin, string? licensePlate = null, DateTime? registrationDate = null, DateTime? expiryDate = null)
        {
            Vin = vin.Value;
            LicensePlate = licensePlate;
            RegistrationDate = registrationDate;
            ExpiryDate = expiryDate;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddPhoto(string photoUrl)
        {
            if (string.IsNullOrWhiteSpace(photoUrl))
                throw new ArgumentException("Photo URL cannot be null or empty", nameof(photoUrl));

            Photos.Add(photoUrl);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemovePhoto(string photoUrl)
        {
            Photos.Remove(photoUrl);
            UpdatedAt = DateTime.UtcNow;
        }

        public void Sell(decimal salePrice)
        {
            if (salePrice < 0)
                throw new ArgumentException("Sale price cannot be negative", nameof(salePrice));

            if (Status != VehicleStatus.Available && Status != VehicleStatus.Reserved)
                throw new InvalidOperationException("Only available or reserved vehicles can be sold");

            Status = VehicleStatus.Sold;
            SalePrice = salePrice;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reserve()
        {
            if (Status != VehicleStatus.Available)
                throw new InvalidOperationException("Only available vehicles can be reserved");

            Status = VehicleStatus.Reserved;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MakeAvailable()
        {
            if (Status == VehicleStatus.Sold)
                throw new InvalidOperationException("Sold vehicles cannot be made available");

            Status = VehicleStatus.Available;
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

        // Computed properties
        public bool IsAvailable => Status == VehicleStatus.Available;

        public bool IsSold => Status == VehicleStatus.Sold;

        public bool IsReserved => Status == VehicleStatus.Reserved;

        public bool IsInService => Status == VehicleStatus.InService;

        public bool IsRegistered => !string.IsNullOrEmpty(Vin) && RegistrationDate.HasValue;
    }
}