using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// PurchaseOrder entity for ordering vehicles from suppliers
    /// </summary>
    public class PurchaseOrder
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("createdBy")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string CreatedBy { get; private set; } = string.Empty;

        [BsonElement("orderDate")]
        public DateTime OrderDate { get; private set; } = DateTime.UtcNow;

        [BsonElement("totalAmount")]
        public decimal TotalAmount { get; private set; }

        [BsonElement("status")]
        public PurchaseOrderStatus Status { get; private set; } = PurchaseOrderStatus.Pending;

        [BsonElement("expectedDeliveryDate")]
        public DateTime? ExpectedDeliveryDate { get; private set; }

        // Internal constructor for MongoDB
        internal PurchaseOrder() { }

        [BsonConstructor]
        public PurchaseOrder(string createdBy, decimal totalAmount, DateTime? expectedDeliveryDate = null)
        {
            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("CreatedBy cannot be null or empty", nameof(createdBy));

            if (totalAmount < 0)
                throw new ArgumentException("Total amount cannot be negative", nameof(totalAmount));

            CreatedBy = createdBy;
            TotalAmount = totalAmount;
            ExpectedDeliveryDate = expectedDeliveryDate;
            OrderDate = DateTime.UtcNow;
        }

        // Domain methods
        public void UpdateTotalAmount(decimal totalAmount)
        {
            if (totalAmount < 0)
                throw new ArgumentException("Total amount cannot be negative", nameof(totalAmount));

            TotalAmount = totalAmount;
        }

        public void UpdateExpectedDeliveryDate(DateTime? expectedDeliveryDate)
        {
            ExpectedDeliveryDate = expectedDeliveryDate;
        }

        public void Complete()
        {
            if (Status == PurchaseOrderStatus.Completed)
                throw new InvalidOperationException("Purchase order is already completed");

            if (Status == PurchaseOrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot complete a cancelled purchase order");

            Status = PurchaseOrderStatus.Completed;
        }

        public void Cancel()
        {
            if (Status == PurchaseOrderStatus.Completed)
                throw new InvalidOperationException("Cannot cancel a completed purchase order");

            if (Status == PurchaseOrderStatus.Cancelled)
                throw new InvalidOperationException("Purchase order is already cancelled");

            Status = PurchaseOrderStatus.Cancelled;
        }

        // Computed properties
        public bool CanBeCompleted => Status == PurchaseOrderStatus.Pending;
        public bool CanBeCancelled => Status == PurchaseOrderStatus.Pending;
    }
}
