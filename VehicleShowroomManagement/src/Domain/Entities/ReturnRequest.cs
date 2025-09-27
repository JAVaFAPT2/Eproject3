using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Return request entity for vehicle return requests
    /// </summary>
    public class ReturnRequest 
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
