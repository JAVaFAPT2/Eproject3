using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// ServiceOrder entity representing vehicle service orders
    /// Manages pre-delivery services and maintenance
    /// </summary>
    public class ServiceOrder : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("vehicleId")]
        [BsonRequired]
        public string VehicleId { get; set; } = string.Empty;

        [BsonElement("customerId")]
        [BsonRequired]
        public string CustomerId { get; set; } = string.Empty;

        [BsonElement("serviceDate")]
        public DateTime ServiceDate { get; set; } = DateTime.UtcNow;

        [BsonElement("status")]
        public string Status { get; set; } = "Scheduled"; // Scheduled, InProgress, Completed, Cancelled

        [BsonElement("totalCost")]
        public decimal TotalCost { get; set; } = 0;

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("serviceType")]
        public string ServiceType { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; };

        // Domain Methods
        public void StartService()
        {
            Status = "InProgress";
            UpdatedAt = DateTime.UtcNow;
        }

        public void CompleteService()
        {
            Status = "Completed";
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelService()
        {
            Status = "Cancelled";
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateTotalCost(decimal cost)
        {
            TotalCost = cost;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reschedule(DateTime newServiceDate)
        {
            ServiceDate = newServiceDate;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanBeStarted()
        {
            return Status == "Scheduled";
        }

        public bool CanBeCompleted()
        {
            return Status == "InProgress";
        }

        public bool CanBeCancelled()
        {
            return Status != "Completed";
        }

        public bool IsCompleted()
        {
            return Status == "Completed";
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