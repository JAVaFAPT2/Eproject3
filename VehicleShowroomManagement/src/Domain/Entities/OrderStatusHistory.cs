using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// OrderStatusHistory entity for tracking order status changes
    /// </summary>
    public class OrderStatusHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("orderId")]
        [BsonRequired]
        public string OrderId { get; private set; } = string.Empty;

        [BsonElement("orderType")]
        [BsonRequired]
        public string OrderType { get; private set; } = "Sales"; // Sales, Service, Purchase

        [BsonElement("previousStatus")]
        public string? PreviousStatus { get; private set; }

        [BsonElement("newStatus")]
        [BsonRequired]
        public string NewStatus { get; private set; } = string.Empty;

        [BsonElement("statusDescription")]
        public string? StatusDescription { get; private set; }

        [BsonElement("changedBy")]
        [BsonRequired]
        public string ChangedBy { get; private set; } = string.Empty;

        [BsonElement("changeReason")]
        public string? ChangeReason { get; private set; }

        [BsonElement("notes")]
        public string? Notes { get; private set; }

        [BsonElement("isSystemGenerated")]
        public bool IsSystemGenerated { get; private set; } = false;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Private constructor for MongoDB
        private OrderStatusHistory() { }

        public OrderStatusHistory(
            string orderId,
            string orderType,
            string newStatus,
            string changedBy,
            string? previousStatus = null,
            string? statusDescription = null,
            string? changeReason = null,
            string? notes = null,
            bool isSystemGenerated = false)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                throw new ArgumentException("Order ID cannot be null or empty", nameof(orderId));

            if (string.IsNullOrWhiteSpace(newStatus))
                throw new ArgumentException("New status cannot be null or empty", nameof(newStatus));

            if (string.IsNullOrWhiteSpace(changedBy))
                throw new ArgumentException("Changed by cannot be null or empty", nameof(changedBy));

            OrderId = orderId;
            OrderType = orderType;
            PreviousStatus = previousStatus;
            NewStatus = newStatus;
            StatusDescription = statusDescription;
            ChangedBy = changedBy;
            ChangeReason = changeReason;
            Notes = notes;
            IsSystemGenerated = isSystemGenerated;
            CreatedAt = DateTime.UtcNow;
        }

        // Static factory methods
        public static OrderStatusHistory ForSalesOrder(
            string orderId, 
            string newStatus, 
            string changedBy,
            string? previousStatus = null,
            string? reason = null)
        {
            return new OrderStatusHistory(orderId, "Sales", newStatus, changedBy, previousStatus, null, reason);
        }

        public static OrderStatusHistory ForServiceOrder(
            string orderId, 
            string newStatus, 
            string changedBy,
            string? previousStatus = null,
            string? reason = null)
        {
            return new OrderStatusHistory(orderId, "Service", newStatus, changedBy, previousStatus, null, reason);
        }

        public static OrderStatusHistory SystemGenerated(
            string orderId,
            string orderType,
            string newStatus,
            string? previousStatus = null,
            string? reason = null)
        {
            return new OrderStatusHistory(orderId, orderType, newStatus, "System", previousStatus, null, reason, null, true);
        }
    }
}