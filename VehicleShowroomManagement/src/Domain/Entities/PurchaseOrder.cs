using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// PurchaseOrder entity representing vehicle purchase orders from company
    /// Manages the process of purchasing vehicles from the manufacturer
    /// </summary>
    public class PurchaseOrder
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("orderNumber")]
        [BsonRequired]
        public string OrderNumber { get; set; } = string.Empty;

        [BsonElement("modelNumber")]
        [BsonRequired]
        public string ModelNumber { get; set; } = string.Empty;

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; } = string.Empty;

        [BsonElement("brand")]
        [BsonRequired]
        public string Brand { get; set; } = string.Empty;

        [BsonElement("price")]
        [BsonRequired]
        public decimal Price { get; set; }

        [BsonElement("quantity")]
        [BsonRequired]
        public int Quantity { get; set; }

        [BsonElement("totalAmount")]
        public decimal TotalAmount { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "Draft"; // Draft, Submitted, Approved, Received, Cancelled

        [BsonElement("orderDate")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [BsonElement("expectedDeliveryDate")]
        public DateTime? ExpectedDeliveryDate { get; set; }

        [BsonElement("actualDeliveryDate")]
        public DateTime? ActualDeliveryDate { get; set; }

        [BsonElement("supplierId")]
        public string? SupplierId { get; set; }

        [BsonElement("supplierName")]
        public string? SupplierName { get; set; }

        [BsonElement("notes")]
        public string? Notes { get; set; }

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
        public void SubmitOrder()
        {
            Status = "Submitted";
            UpdatedAt = DateTime.UtcNow;
        }

        public void ApproveOrder()
        {
            Status = "Approved";
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsReceived()
        {
            Status = "Received";
            ActualDeliveryDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelOrder()
        {
            Status = "Cancelled";
            UpdatedAt = DateTime.UtcNow;
        }

        public void CalculateTotalAmount()
        {
            TotalAmount = Price * Quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanBeModified()
        {
            return Status == "Draft" || Status == "Submitted";
        }

        public bool CanBeCancelled()
        {
            return Status != "Received" && Status != "Cancelled";
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
