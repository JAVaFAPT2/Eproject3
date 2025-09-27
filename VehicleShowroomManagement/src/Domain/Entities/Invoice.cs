using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Invoice entity representing billing documents for sales orders and service orders
    /// </summary>
    public class Invoice 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("serviceOrderId")]
        public string? ServiceOrderId { get; set; }

        [BsonElement("salesOrderId")]
        public string? SalesOrderId { get; set; }

        [BsonElement("invoiceNumber")]
        [BsonRequired]
        public string InvoiceNumber { get; set; } = string.Empty;

        [BsonElement("invoiceType")]
        public string InvoiceType { get; set; } = "Sales"; // Sales, Service

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

        public void SetForSalesOrder(string salesOrderId)
        {
            SalesOrderId = salesOrderId;
            ServiceOrderId = null;
            InvoiceType = "Sales";
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetForServiceOrder(string serviceOrderId)
        {
            ServiceOrderId = serviceOrderId;
            SalesOrderId = null;
            InvoiceType = "Service";
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

        public bool IsServiceInvoice => InvoiceType == "Service" && !string.IsNullOrEmpty(ServiceOrderId);
        
        public bool IsSalesInvoice => InvoiceType == "Sales" && !string.IsNullOrEmpty(SalesOrderId);
        
        public string GetOrderId => IsServiceInvoice ? ServiceOrderId! : SalesOrderId ?? string.Empty;

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