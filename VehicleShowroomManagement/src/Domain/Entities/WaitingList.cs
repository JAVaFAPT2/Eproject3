using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// WaitingList entity representing customer waiting list details
    /// Manages customer waiting list for specific vehicle models or types
    /// </summary>
    public class WaitingList : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("waitingListNumber")]
        [BsonRequired]
        public string WaitingListNumber { get; set; } = string.Empty;

        [BsonElement("customerId")]
        [BsonRequired]
        public string CustomerId { get; set; } = string.Empty;

        [BsonElement("modelId")]
        public string? ModelId { get; set; }

        [BsonElement("brand")]
        public string? Brand { get; set; }

        [BsonElement("modelName")]
        public string? ModelName { get; set; }

        [BsonElement("vehicleType")]
        public string? VehicleType { get; set; }

        [BsonElement("preferredColor")]
        public string? PreferredColor { get; set; }

        [BsonElement("preferredYear")]
        public int? PreferredYear { get; set; }

        [BsonElement("maxPrice")]
        public decimal? MaxPrice { get; set; }

        [BsonElement("minPrice")]
        public decimal? MinPrice { get; set; }

        [BsonElement("priority")]
        public int Priority { get; set; } = 1; // 1 = High, 2 = Medium, 3 = Low

        [BsonElement("position")]
        public int Position { get; set; } = 1;

        [BsonElement("status")]
        public string Status { get; set; } = "Active"; // Active, Notified, Converted, Cancelled, Expired

        [BsonElement("entryDate")]
        [BsonRequired]
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [BsonElement("expectedAvailabilityDate")]
        public DateTime? ExpectedAvailabilityDate { get; set; }

        [BsonElement("lastContactDate")]
        public DateTime? LastContactDate { get; set; }

        [BsonElement("nextContactDate")]
        public DateTime? NextContactDate { get; set; }

        [BsonElement("contactMethod")]
        public string ContactMethod { get; set; } = "Phone"; // Phone, Email, SMS, WhatsApp

        [BsonElement("contactFrequency")]
        public string ContactFrequency { get; set; } = "Weekly"; // Daily, Weekly, Monthly, OnAvailability

        [BsonElement("isFlexible")]
        public bool IsFlexible { get; set; } = false; // Flexible on color, year, etc.

        [BsonElement("flexibilityNotes")]
        public string? FlexibilityNotes { get; set; }

        [BsonElement("specialRequirements")]
        public string? SpecialRequirements { get; set; }

        [BsonElement("notes")]
        public string? Notes { get; set; }

        [BsonElement("convertedToAllotment")]
        public bool ConvertedToAllotment { get; set; } = false;

        [BsonElement("allotmentId")]
        public string? AllotmentId { get; set; }

        [BsonElement("conversionDate")]
        public DateTime? ConversionDate { get; set; }

        [BsonElement("cancellationReason")]
        public string? CancellationReason { get; set; }

        [BsonElement("cancelledDate")]
        public DateTime? CancelledDate { get; set; }

        [BsonElement("cancelledBy")]
        public string? CancelledBy { get; set; }

        [BsonElement("assignedTo")]
        public string? AssignedTo { get; set; } // Sales person assigned to handle this waiting list entry

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

        // Domain Methods
        public void ActivateWaitingList()
        {
            Status = "Active";
            UpdatedAt = DateTime.UtcNow;
        }

        public void NotifyCustomer()
        {
            Status = "Notified";
            LastContactDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ConvertToAllotment(string allotmentId)
        {
            Status = "Converted";
            ConvertedToAllotment = true;
            AllotmentId = allotmentId;
            ConversionDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelWaitingList(string reason, string cancelledBy)
        {
            Status = "Cancelled";
            CancellationReason = reason;
            CancelledBy = cancelledBy;
            CancelledDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ExpireWaitingList()
        {
            Status = "Expired";
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePosition(int newPosition)
        {
            Position = newPosition;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePriority(int newPriority)
        {
            Priority = newPriority;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AssignToSalesPerson(string salesPersonId)
        {
            AssignedTo = salesPersonId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateContactInfo(DateTime nextContactDate, string contactMethod)
        {
            NextContactDate = nextContactDate;
            ContactMethod = contactMethod;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsActive()
        {
            return Status == "Active";
        }

        public bool CanBeConverted()
        {
            return Status == "Active" && !ConvertedToAllotment;
        }

        public bool CanBeCancelled()
        {
            return Status == "Active" && !ConvertedToAllotment;
        }

        public bool IsEligibleForVehicle(string brand, string model, string color, int year, decimal price)
        {
            if (!IsActive()) return false;

            // Check brand match
            if (!string.IsNullOrEmpty(Brand) && !Brand.Equals(brand, StringComparison.OrdinalIgnoreCase))
                return false;

            // Check model match
            if (!string.IsNullOrEmpty(ModelName) && !ModelName.Equals(model, StringComparison.OrdinalIgnoreCase))
                return false;

            // Check color match (if not flexible)
            if (!IsFlexible && !string.IsNullOrEmpty(PreferredColor) && !PreferredColor.Equals(color, StringComparison.OrdinalIgnoreCase))
                return false;

            // Check year match (if not flexible)
            if (!IsFlexible && PreferredYear.HasValue && PreferredYear.Value != year)
                return false;

            // Check price range
            if (MinPrice.HasValue && price < MinPrice.Value)
                return false;

            if (MaxPrice.HasValue && price > MaxPrice.Value)
                return false;

            return true;
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
