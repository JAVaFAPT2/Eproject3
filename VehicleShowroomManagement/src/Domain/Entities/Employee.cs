using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Employee entity representing system employees (HR, Dealers)
    /// Implements role-based access control
    /// </summary>
    public class Employee : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("employeeId")]
        [BsonRequired]
        public string EmployeeId { get; set; } = string.Empty;

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; } = string.Empty;

        [BsonElement("role")]
        [BsonRequired]
        public string Role { get; set; } = string.Empty; // "Dealer", "HR"

        [BsonElement("position")]
        public string? Position { get; set; }

        [BsonElement("hireDate")]
        public DateTime HireDate { get; set; } = DateTime.UtcNow;

        [BsonElement("status")]
        public string Status { get; set; } = "Active";

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Computed Properties
        [BsonIgnore]
        public bool IsActive => Status == "Active";

        [BsonIgnore]
        public bool IsDealer => Role == "Dealer";

        [BsonIgnore]
        public bool IsHR => Role == "HR";

        // Domain Methods
        public void UpdateProfile(string name, string? position)
        {
            Name = name;
            Position = position;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeStatus(string status)
        {
            Status = status;
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

