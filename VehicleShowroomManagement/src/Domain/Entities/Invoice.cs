using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Invoice entity representing billing documents for sales orders
    /// </summary>
    public class Invoice : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("salesOrderId")]
        [BsonRequired]
        public string SalesOrderId { get; set; } = string.Empty;

        [BsonElement("invoiceNumber")]
        [BsonRequired]
        public string InvoiceNumber { get; set; } = string.Empty;

        [BsonElement("invoiceDate")]
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

        [BsonElement("subtotal")]
        public decimal Subtotal { get; set; }

        [BsonElement("taxAmount")]
        public decimal TaxAmount { get; set; }

        [BsonElement("totalAmount")]
        public decimal TotalAmount { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "Unpaid"; // Unpaid, Paid, PartiallyPaid, Overdue, Cancelled

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Private constructor for MongoDB
        private Invoice() { }

        public Invoice(string salesOrderId, string invoiceNumber, string customerId, DateTime invoiceDate, DateTime dueDate, decimal subtotal, decimal taxAmount, decimal totalAmount)
        {
            if (string.IsNullOrWhiteSpace(salesOrderId))
                throw new ArgumentException("Sales order ID cannot be null or empty", nameof(salesOrderId));

            if (string.IsNullOrWhiteSpace(invoiceNumber))
                throw new ArgumentException("Invoice number cannot be null or empty", nameof(invoiceNumber));

            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));

            if (subtotal < 0)
                throw new ArgumentException("Subtotal cannot be negative", nameof(subtotal));

            if (taxAmount < 0)
                throw new ArgumentException("Tax amount cannot be negative", nameof(taxAmount));

            if (totalAmount < 0)
                throw new ArgumentException("Total amount cannot be negative", nameof(totalAmount));

            SalesOrderId = salesOrderId;
            InvoiceNumber = invoiceNumber;
            CustomerId = customerId;
            InvoiceDate = invoiceDate;
            DueDate = dueDate;
            Subtotal = subtotal;
            TaxAmount = taxAmount;
            TotalAmount = totalAmount;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // References to other documents
        [BsonElement("paymentIds")]
        public List<string> PaymentIds { get; set; } = new List<string>();

        // Domain Methods
        public void CalculateTotals(decimal subtotal, decimal taxRate)
        {
            Subtotal = subtotal;
            TaxAmount = subtotal * (taxRate / 100);
            TotalAmount = Subtotal + TaxAmount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsPaid()
        {
            Status = "Paid";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsPartiallyPaid()
        {
            Status = "PartiallyPaid";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsOverdue()
        {
            Status = "Overdue";
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            Status = "Cancelled";
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsPaid()
        {
            return Status == "Paid";
        }

        public bool IsUnpaid()
        {
            return Status == "Unpaid";
        }

        public bool IsOverdue()
        {
            return Status == "Overdue";
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