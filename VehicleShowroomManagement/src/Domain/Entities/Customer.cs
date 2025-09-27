using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Customer aggregate root for customer management
    /// </summary>
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("customerId")]
        [BsonRequired]
        public string CustomerId { get; private set; } = string.Empty;

        [BsonElement("firstName")]
        [BsonRequired]
        public string FirstName { get; private set; } = string.Empty;

        [BsonElement("lastName")]
        [BsonRequired]
        public string LastName { get; private set; } = string.Empty;

        [BsonElement("email")]
        public string? Email { get; private set; }

        [BsonElement("phone")]
        public string? Phone { get; private set; }

        [BsonElement("address")]
        public Address? Address { get; private set; }

        [BsonElement("cccd")]
        public string? Cccd { get; private set; } // Citizen ID

        [BsonElement("isActive")]
        public bool IsActive { get; private set; } = true;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Private constructor for MongoDB
        private Customer() { }

        public Customer(string customerId, string firstName, string lastName, string? email = null, string? phone = null, Address? address = null, string? cccd = null)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be null or empty", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));

            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
            Cccd = cccd;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain methods
        public void UpdateProfile(string firstName, string lastName, string? email = null, string? phone = null, Address? address = null, string? cccd = null)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be null or empty", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
            Cccd = cccd;
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

        // Computed properties
        public string FullName => $"{FirstName} {LastName}";
    }
}