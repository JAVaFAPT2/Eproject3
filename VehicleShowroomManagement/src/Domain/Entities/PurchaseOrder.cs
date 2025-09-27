using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// PurchaseOrder entity representing vehicle purchase orders from company
    /// Manages the process of purchasing vehicles from the manufacturer
    /// </summary>
    public class PurchaseOrder : IEntity, IAuditableEntity, ISoftDelete
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

        // Private constructor for MongoDB
        private PurchaseOrder() { }

        public PurchaseOrder(
            string orderNumber,
            string modelNumber,
            string name,
            string brand,
            decimal price,
            int quantity,
            decimal totalAmount,
            string? supplierId,
            string? supplierName,
            string? notes,
            DateTime? expectedDeliveryDate,
            string createdBy)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
                throw new ArgumentException("Order number cannot be null or empty", nameof(orderNumber));

            if (string.IsNullOrWhiteSpace(modelNumber))
                throw new ArgumentException("Model number cannot be null or empty", nameof(modelNumber));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Brand cannot be null or empty", nameof(brand));

            if (price < 0)
                throw new ArgumentException("Price cannot be negative", nameof(price));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            if (totalAmount < 0)
                throw new ArgumentException("Total amount cannot be negative", nameof(totalAmount));

            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("Created by cannot be null or empty", nameof(createdBy));

            OrderNumber = orderNumber;
            ModelNumber = modelNumber;
            Name = name;
            Brand = brand;
            Price = price;
            Quantity = quantity;
            TotalAmount = totalAmount;
            SupplierId = supplierId;
            SupplierName = supplierName;
            Notes = notes;
            ExpectedDeliveryDate = expectedDeliveryDate;
            CreatedBy = createdBy;
            OrderDate = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

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
