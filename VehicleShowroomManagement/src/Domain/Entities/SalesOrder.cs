using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// SalesOrder entity representing vehicle sales orders
    /// Central entity for managing vehicle sales transactions
    /// </summary>
    public class SalesOrder : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("customerId")]
        [BsonRequired]
        public string CustomerId { get; set; } = string.Empty;

        [BsonElement("salesPersonId")]
        [BsonRequired]
        public string SalesPersonId { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = "Draft"; // Draft, Confirmed, Invoiced, Completed, Cancelled

        [BsonElement("orderDate")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [BsonElement("totalAmount")]
        public decimal TotalAmount { get; set; } = 0;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // References to other documents
        [BsonElement("salesOrderItemIds")]
        public List<string> SalesOrderItemIds { get; set; } = new List<string>();

        // Navigation property for domain operations
        public List<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();

        [BsonElement("invoiceIds")]
        public List<string> InvoiceIds { get; set; } = new List<string>();

        // Domain Methods
        public void ConfirmOrder()
        {
            Status = "Confirmed";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsInvoiced()
        {
            Status = "Invoiced";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsCompleted()
        {
            Status = "Completed";
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelOrder()
        {
            Status = "Cancelled";
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateTotalAmount(decimal amount)
        {
            TotalAmount = amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanBeModified()
        {
            return Status == "Draft" || Status == "Confirmed";
        }

        public bool CanBeCancelled()
        {
            return Status != "Completed" && Status != "Cancelled";
        }

        public bool IsConfirmed()
        {
            return Status == "Confirmed";
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