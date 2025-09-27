using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Allotment entity representing vehicle allotment details
    /// Manages vehicle allocation to customers and sales representatives
    /// </summary>
    public class Allotment : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("allotmentNumber")]
        [BsonRequired]
        public string AllotmentNumber { get; set; } = string.Empty;

        [BsonElement("vehicleId")]
        [BsonRequired]
        public string VehicleId { get; set; } = string.Empty;

        [BsonElement("customerId")]
        [BsonRequired]
        public string CustomerId { get; set; } = string.Empty;

        [BsonElement("salesPersonId")]
        [BsonRequired]
        public string SalesPersonId { get; set; } = string.Empty;

        [BsonElement("allotmentDate")]
        [BsonRequired]
        public DateTime AllotmentDate { get; set; } = DateTime.UtcNow;

        [BsonElement("validUntil")]
        [BsonRequired]
        public DateTime ValidUntil { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "Active"; // Active, Expired, Converted, Cancelled

        [BsonElement("allotmentType")]
        public string AllotmentType { get; set; } = "Reservation"; // Reservation, Hold, Priority

        [BsonElement("priority")]
        public int Priority { get; set; } = 1; // 1 = High, 2 = Medium, 3 = Low

        [BsonElement("reservationAmount")]
        public decimal ReservationAmount { get; set; } = 0;

        [BsonElement("reservationPaid")]
        public bool ReservationPaid { get; set; } = false;

        [BsonElement("paymentMethod")]
        public string? PaymentMethod { get; set; }

        [BsonElement("paymentReference")]
        public string? PaymentReference { get; set; }

        [BsonElement("specialConditions")]
        public string? SpecialConditions { get; set; }

        [BsonElement("notes")]
        public string? Notes { get; set; }

        [BsonElement("convertedToOrder")]
        public bool ConvertedToOrder { get; set; } = false;

        [BsonElement("orderId")]
        public string? OrderId { get; set; }

        [BsonElement("conversionDate")]
        public DateTime? ConversionDate { get; set; }

        [BsonElement("cancellationReason")]
        public string? CancellationReason { get; set; }

        [BsonElement("cancelledDate")]
        public DateTime? CancelledDate { get; set; }

        [BsonElement("cancelledBy")]
        public string? CancelledBy { get; set; }

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
        public void ActivateAllotment()
        {
            Status = "Active";
            UpdatedAt = DateTime.UtcNow;
        }

        public void ExpireAllotment()
        {
            Status = "Expired";
            UpdatedAt = DateTime.UtcNow;
        }

        public void ConvertToOrder(string orderId)
        {
            Status = "Converted";
            ConvertedToOrder = true;
            OrderId = orderId;
            ConversionDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelAllotment(string reason, string cancelledBy)
        {
            Status = "Cancelled";
            CancellationReason = reason;
            CancelledBy = cancelledBy;
            CancelledDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ExtendAllotment(DateTime newValidUntil)
        {
            ValidUntil = newValidUntil;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkReservationPaid(string paymentMethod, string? paymentReference = null)
        {
            ReservationPaid = true;
            PaymentMethod = paymentMethod;
            PaymentReference = paymentReference;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsExpired()
        {
            return ValidUntil < DateTime.UtcNow;
        }

        public bool IsActive()
        {
            return Status == "Active" && !IsExpired();
        }

        public bool CanBeConverted()
        {
            return Status == "Active" && !IsExpired() && !ConvertedToOrder;
        }

        public bool CanBeCancelled()
        {
            return Status == "Active" && !ConvertedToOrder;
        }

        public bool CanBeExtended()
        {
            return Status == "Active" && !ConvertedToOrder;
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
