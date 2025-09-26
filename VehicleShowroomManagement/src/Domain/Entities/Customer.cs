using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Customer entity representing customers who purchase vehicles
    /// </summary>
    public class Customer : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("firstName")]
        [BsonRequired]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("lastName")]
        [BsonRequired]
        public string LastName { get; set; } = string.Empty;

        [BsonElement("email")]
        [BsonRequired]
        public string Email { get; set; } = string.Empty;

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }

        [BsonElement("city")]
        public string? City { get; set; }

        [BsonElement("state")]
        public string? State { get; set; }

        [BsonElement("zipCode")]
        public string? ZipCode { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // References to other documents
        [BsonElement("salesOrderIds")]
        public List<string> SalesOrderIds { get; set; } = new List<string>();

        [BsonElement("serviceOrderIds")]
        public List<string> ServiceOrderIds { get; set; } = new List<string>();

        // Computed Properties
        [BsonIgnore]
        public string FullName => $"{FirstName} {LastName}";

        [BsonIgnore]
        public AddressInfo? CustomerAddress
        {
            get
            {
                if (string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(State))
                    return null;

                return new AddressInfo(Address, City, State, ZipCode);
            }
        }

        // Domain Methods
        public void UpdateProfile(string firstName, string lastName, string email, string? phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateAddress(string? address, string? city, string? state, string? zipCode)
        {
            Address = address;
            City = city;
            State = state;
            ZipCode = zipCode;
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
    /// Embedded address information within Customer document
    /// </summary>
    public class AddressInfo
    {
        [BsonElement("street")]
        [BsonRequired]
        public string Street { get; set; } = string.Empty;

        [BsonElement("city")]
        [BsonRequired]
        public string City { get; set; } = string.Empty;

        [BsonElement("state")]
        [BsonRequired]
        public string State { get; set; } = string.Empty;

        [BsonElement("zipCode")]
        public string? ZipCode { get; set; }

        public AddressInfo(string street, string city, string state, string? zipCode)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        [BsonIgnore]
        public string FullAddress => $"{Street}, {City}, {State} {(string.IsNullOrEmpty(ZipCode) ? "" : ZipCode)}";
    }
}