using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Payment entity representing payments made against invoices
    /// </summary>
    public class Payment 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("invoiceId")]
        [BsonRequired]
        public string InvoiceId { get; set; } = string.Empty;

        [BsonElement("amount")]
        [BsonRequired]
        public decimal Amount { get; set; }

        [BsonElement("paymentMethod")]
        [BsonRequired]
        public string PaymentMethod { get; set; } = string.Empty; // Cash, CreditCard, BankTransfer, Check

        [BsonElement("paymentDate")]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [BsonElement("referenceNumber")]
        public string? ReferenceNumber { get; set; }

        [BsonElement("status")]
        [BsonRequired]
        public string Status { get; set; } = "Completed"; // Completed, Pending, Failed, Cancelled

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Domain Methods
        public void MarkAsCompleted()
        {
            Status = "Completed";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsPending()
        {
            Status = "Pending";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsFailed()
        {
            Status = "Failed";
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            Status = "Cancelled";
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsCompleted()
        {
            return Status == "Completed";
        }

        public bool IsPending()
        {
            return Status == "Pending";
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