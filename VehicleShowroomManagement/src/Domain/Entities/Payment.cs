using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Payment entity representing payments made against invoices
    /// </summary>
    public class Payment : IEntity, IAuditableEntity, ISoftDelete
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

        [BsonElement("salesOrderId")]
        [BsonRequired]
        public string SalesOrderId { get; set; } = string.Empty;

        // Private constructor for MongoDB
        private Payment() { }

        public Payment(string invoiceId, string salesOrderId, decimal amount, string paymentMethod, DateTime paymentDate, string? referenceNumber = null)
        {
            if (string.IsNullOrWhiteSpace(invoiceId))
                throw new ArgumentException("Invoice ID cannot be null or empty", nameof(invoiceId));

            if (string.IsNullOrWhiteSpace(salesOrderId))
                throw new ArgumentException("Sales order ID cannot be null or empty", nameof(salesOrderId));

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero", nameof(amount));

            if (string.IsNullOrWhiteSpace(paymentMethod))
                throw new ArgumentException("Payment method cannot be null or empty", nameof(paymentMethod));

            InvoiceId = invoiceId;
            SalesOrderId = salesOrderId;
            Amount = amount;
            PaymentMethod = paymentMethod;
            PaymentDate = paymentDate;
            ReferenceNumber = referenceNumber;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

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