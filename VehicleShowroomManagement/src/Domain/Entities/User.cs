using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// User entity - unified for all user types (customers, employees, dealers, HR, admin)
    /// </summary>
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("username")]
        [BsonRequired]
        public string Username { get; private set; } = string.Empty;

        [BsonElement("passwordHash")]
        [BsonRequired]
        public string PasswordHash { get; private set; } = string.Empty;

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; private set; } = string.Empty;

        [BsonElement("email")]
        [BsonRequired]
        public string Email { get; private set; } = string.Empty;

        [BsonElement("phone")]
        public string? Phone { get; private set; }

        [BsonElement("address")]
        public string? Address { get; private set; }

        [BsonElement("roleId")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string RoleId { get; private set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; private set; } = "Active";

        [BsonElement("hireDate")]
        public DateTime? HireDate { get; private set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Internal constructor for MongoDB
        internal User() { }

        [BsonConstructor]
        public User(string username, string passwordHash, string name, string email, string roleId, 
            string? phone = null, string? address = null, DateTime? hireDate = null)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty", nameof(username));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be null or empty", nameof(passwordHash));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty", nameof(email));

            if (string.IsNullOrWhiteSpace(roleId))
                throw new ArgumentException("Role ID cannot be null or empty", nameof(roleId));

            Username = username;
            PasswordHash = passwordHash;
            Name = name;
            Email = email;
            RoleId = roleId;
            Phone = phone;
            Address = address;
            HireDate = hireDate;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain methods
        public void UpdateProfile(string name, string email, string? phone = null, string? address = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty", nameof(email));

            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password hash cannot be null or empty", nameof(newPasswordHash));

            PasswordHash = newPasswordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeRole(string newRoleId)
        {
            if (string.IsNullOrWhiteSpace(newRoleId))
                throw new ArgumentException("Role ID cannot be null or empty", nameof(newRoleId));

            RoleId = newRoleId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            Status = "Active";
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            Status = "Inactive";
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            DeletedAt = DateTime.UtcNow;
            Status = "Deleted";
            UpdatedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            DeletedAt = null;
            Status = "Active";
            UpdatedAt = DateTime.UtcNow;
        }

        // Computed properties
        public bool IsActive => Status == "Active";
        public bool IsDeleted => DeletedAt.HasValue;
        public bool IsEmployee => HireDate.HasValue;
    }
}
