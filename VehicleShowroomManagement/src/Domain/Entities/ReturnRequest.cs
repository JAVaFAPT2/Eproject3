using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Return request entity for vehicle return requests
    /// </summary>
    public class ReturnRequest : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("orderId")]
        [BsonRequired]
        public string OrderId { get; set; } = string.Empty;

        [BsonElement("customerId")]
        [BsonRequired]
        public string CustomerId { get; set; } = string.Empty;

        [BsonElement("vehicleId")]
        [BsonRequired]
        public string VehicleId { get; set; } = string.Empty;

        [BsonElement("reason")]
        [BsonRequired]
        public string Reason { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("refundAmount")]
        public decimal RefundAmount { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "PENDING"; // PENDING, APPROVED, REJECTED, COMPLETED

        [BsonElement("processedBy")]
        public string? ProcessedBy { get; set; }

        [BsonElement("processedAt")]
        public DateTime? ProcessedAt { get; set; }

        [BsonElement("notes")]
        public string Notes { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Private constructor for MongoDB
        private ReturnRequest() { }

        public ReturnRequest(string orderId, string customerId, string vehicleId, string reason, string description = "", decimal refundAmount = 0)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                throw new ArgumentException("Order ID cannot be null or empty", nameof(orderId));

            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));

            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be null or empty", nameof(vehicleId));

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Reason cannot be null or empty", nameof(reason));

            if (refundAmount < 0)
                throw new ArgumentException("Refund amount cannot be negative", nameof(refundAmount));

            OrderId = orderId;
            CustomerId = customerId;
            VehicleId = vehicleId;
            Reason = reason;
            Description = description;
            RefundAmount = refundAmount;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain Methods
        public void Approve(string processedBy, string notes = "")
        {
            Status = "APPROVED";
            ProcessedBy = processedBy;
            ProcessedAt = DateTime.UtcNow;
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reject(string processedBy, string notes = "")
        {
            Status = "REJECTED";
            ProcessedBy = processedBy;
            ProcessedAt = DateTime.UtcNow;
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete(string processedBy, string notes = "")
        {
            Status = "COMPLETED";
            ProcessedBy = processedBy;
            ProcessedAt = DateTime.UtcNow;
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetRefundAmount(decimal amount)
        {
            RefundAmount = amount;
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

        // Business Rules
        public bool CanBeProcessed => Status == "PENDING";
        public bool IsApproved => Status == "APPROVED";
        public bool IsRejected => Status == "REJECTED";
        public bool IsCompleted => Status == "COMPLETED";
    }
}
