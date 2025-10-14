using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// ServiceOrder entity for vehicle services
    /// </summary>
    public class ServiceOrder
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("orderId")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string OrderId { get; private set; } = string.Empty;

        [BsonElement("createdBy")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string CreatedBy { get; private set; } = string.Empty;

        [BsonElement("serviceDate")]
        public DateTime? ServiceDate { get; private set; }

        [BsonElement("appointmentDate")]
        public DateTime? AppointmentDate { get; private set; }

        [BsonElement("description")]
        public string? Description { get; private set; }

        [BsonElement("cost")]
        public decimal Cost { get; private set; }

        [BsonElement("type")]
        public ServiceType Type { get; private set; }

        [BsonElement("status")]
        public ServiceOrderStatus Status { get; private set; } = ServiceOrderStatus.Scheduled;

        [BsonElement("licensePlate")]
        public string? LicensePlate { get; private set; }

        // Internal constructor for MongoDB
        internal ServiceOrder() { }

        [BsonConstructor]
        public ServiceOrder(string orderId, string createdBy, ServiceType type, decimal cost, 
            DateTime? appointmentDate = null, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                throw new ArgumentException("Order ID cannot be null or empty", nameof(orderId));

            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("CreatedBy cannot be null or empty", nameof(createdBy));

            if (cost < 0)
                throw new ArgumentException("Cost cannot be negative", nameof(cost));

            OrderId = orderId;
            CreatedBy = createdBy;
            Type = type;
            Cost = cost;
            AppointmentDate = appointmentDate;
            Description = description;
        }

        // Domain methods
        public void UpdateAppointmentDate(DateTime? appointmentDate)
        {
            AppointmentDate = appointmentDate;
        }

        public void UpdateDescription(string? description)
        {
            Description = description;
        }

        public void UpdateCost(decimal cost)
        {
            if (cost < 0)
                throw new ArgumentException("Cost cannot be negative", nameof(cost));

            Cost = cost;
        }

        public void Complete()
        {
            if (Status == ServiceOrderStatus.Completed)
                throw new InvalidOperationException("Service order is already completed");

            if (Status == ServiceOrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot complete a cancelled service order");

            Status = ServiceOrderStatus.Completed;
            ServiceDate = DateTime.UtcNow;
        }

    public void Cancel()
    {
        if (Status == ServiceOrderStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed service order");

        if (Status == ServiceOrderStatus.Cancelled)
            throw new InvalidOperationException("Service order is already cancelled");

        Status = ServiceOrderStatus.Cancelled;
    }

    public void UpdateStatus(ServiceOrderStatus newStatus, string? licensePlate = null)
    {
        if (newStatus == ServiceOrderStatus.Completed)
        {
            Complete();
        }
        else if (newStatus == ServiceOrderStatus.Cancelled)
        {
            Cancel();
        }
        else
        {
            Status = newStatus;
        }
        
        if (!string.IsNullOrWhiteSpace(licensePlate))
        {
            LicensePlate = licensePlate;
        }
    }

    // Computed properties
    public bool IsScheduled => Status == ServiceOrderStatus.Scheduled;
    public bool IsCompleted => Status == ServiceOrderStatus.Completed;
    public bool IsCancelled => Status == ServiceOrderStatus.Cancelled;
}
}
