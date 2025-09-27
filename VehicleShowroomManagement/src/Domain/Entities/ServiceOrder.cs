using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// ServiceOrder entity representing vehicle service orders
    /// Manages pre-delivery services and maintenance
    /// </summary>
    public class ServiceOrder 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

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