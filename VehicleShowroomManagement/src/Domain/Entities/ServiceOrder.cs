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

        [BsonElement("serviceOrderNumber")]
        [BsonRequired]
        public string ServiceOrderNumber { get; set; } = string.Empty;

        [BsonElement("serviceOrderId")]
        [BsonRequired]
        public string ServiceOrderId { get; set; } = string.Empty;

        [BsonElement("salesOrderId")]
        [BsonRequired]
        public string SalesOrderId { get; set; } = string.Empty;

        [BsonElement("employeeId")]
        [BsonRequired]
        public string EmployeeId { get; set; } = string.Empty;

        [BsonElement("serviceDate")]
        public DateTime ServiceDate { get; set; } = DateTime.UtcNow;

        [BsonElement("description")]
        [BsonRequired]
        public string Description { get; set; } = string.Empty;

        [BsonElement("cost")]
        [BsonRequired]
        public decimal Cost { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Private constructor for MongoDB
        private ServiceOrder() { }

        public ServiceOrder(string serviceOrderNumber, string salesOrderId, string employeeId, DateTime serviceDate, string description, decimal cost, string? notes = null)
        {
            if (string.IsNullOrWhiteSpace(serviceOrderNumber))
                throw new ArgumentException("Service order number cannot be null or empty", nameof(serviceOrderNumber));

            if (string.IsNullOrWhiteSpace(salesOrderId))
                throw new ArgumentException("Sales order ID cannot be null or empty", nameof(salesOrderId));

            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("Employee ID cannot be null or empty", nameof(employeeId));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be null or empty", nameof(description));

            if (cost < 0)
                throw new ArgumentException("Cost cannot be negative", nameof(cost));

            ServiceOrderNumber = serviceOrderNumber;
            SalesOrderId = salesOrderId;
            EmployeeId = employeeId;
            ServiceDate = serviceDate;
            Description = description;
            Cost = cost;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain Methods
        public void UpdateCost(decimal cost)
        {
            Cost = cost;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateServiceDate(DateTime serviceDate)
        {
            ServiceDate = serviceDate;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
            UpdatedAt = DateTime.UtcNow;
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