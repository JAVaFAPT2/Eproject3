using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// User entity representing system users (HR, Dealers, Customers)
    /// Implements role-based access control
    /// </summary>
    public class User : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("username")]
        [BsonRequired]
        public string Username { get; set; } = string.Empty;

        [BsonElement("email")]
        [BsonRequired]
        public string Email { get; set; } = string.Empty;

        [BsonElement("passwordHash")]
        [BsonRequired]
        public string PasswordHash { get; set; } = string.Empty;

        [BsonElement("firstName")]
        [BsonRequired]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("lastName")]
        [BsonRequired]
        public string LastName { get; set; } = string.Empty;

        [BsonElement("roleId")]
        [BsonRequired]
        public string RoleId { get; set; } = string.Empty;

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Embedded role information
        [BsonElement("role")]
        public RoleInfo? Role { get; set; }

        // References to other documents
        [BsonElement("userRoles")]
        public List<string> UserRoleIds { get; set; } = new List<string>();

        [BsonElement("salesOrderIds")]
        public List<string> SalesOrderIds { get; set; } = new List<string>();

        // Computed Properties
        [BsonIgnore]
        public string FullName => $"{FirstName} {LastName}";

        [BsonIgnore]
        public bool IsHr => Role?.RoleName == "HR";

        [BsonIgnore]
        public bool IsDealer => Role?.RoleName == "Dealer";

        [BsonIgnore]
        public bool IsAdmin => Role?.RoleName == "Admin";

        // Domain Methods
        public void UpdateProfile(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
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

    /// <summary>
    /// Embedded role information within User document
    /// </summary>
    public class RoleInfo
    {
        [BsonElement("roleId")]
        [BsonRequired]
        public string RoleId { get; set; } = string.Empty;

        [BsonElement("roleName")]
        [BsonRequired]
        public string RoleName { get; set; } = string.Empty;

        [BsonElement("description")]
        public string? Description { get; set; }
    }
}