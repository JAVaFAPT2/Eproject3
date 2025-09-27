using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Supplier aggregate root for supplier management in purchase operations
    /// </summary>
    public class Supplier
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("supplierId")]
        [BsonRequired]
        public string SupplierId { get; private set; } = string.Empty;

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; private set; } = string.Empty;

        [BsonElement("contactPerson")]
        public string? ContactPerson { get; private set; }

        [BsonElement("email")]
        public string? Email { get; private set; }

        [BsonElement("phone")]
        public string? Phone { get; private set; }

        [BsonElement("address")]
        public Address? Address { get; private set; }

        [BsonElement("taxNumber")]
        public string? TaxNumber { get; private set; }

        [BsonElement("paymentTerms")]
        public string? PaymentTerms { get; private set; }

        [BsonElement("isActive")]
        public bool IsActive { get; private set; } = true;

        [BsonElement("rating")]
        public int Rating { get; private set; } = 5; // 1-5 scale

        [BsonElement("specialties")]
        public List<string> Specialties { get; private set; } = new List<string>();

        [BsonElement("notes")]
        public string? Notes { get; private set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Private constructor for MongoDB
        private Supplier() { }

        public Supplier(string supplierId, string name, string? contactPerson = null, string? email = null, string? phone = null, Address? address = null, string? taxNumber = null)
        {
            if (string.IsNullOrWhiteSpace(supplierId))
                throw new ArgumentException("Supplier ID cannot be null or empty", nameof(supplierId));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Supplier name cannot be null or empty", nameof(name));

            SupplierId = supplierId;
            Name = name;
            ContactPerson = contactPerson;
            Email = email;
            Phone = phone;
            Address = address;
            TaxNumber = taxNumber;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain methods
        public void UpdateContactInfo(string? contactPerson = null, string? email = null, string? phone = null, Address? address = null)
        {
            ContactPerson = contactPerson;
            Email = email;
            Phone = phone;
            Address = address;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateBusinessInfo(string? taxNumber = null, string? paymentTerms = null)
        {
            TaxNumber = taxNumber;
            PaymentTerms = paymentTerms;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetRating(int rating)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5", nameof(rating));

            Rating = rating;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddSpecialty(string specialty)
        {
            if (string.IsNullOrWhiteSpace(specialty))
                throw new ArgumentException("Specialty cannot be null or empty", nameof(specialty));

            if (!Specialties.Contains(specialty))
            {
                Specialties.Add(specialty);
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void RemoveSpecialty(string specialty)
        {
            Specialties.Remove(specialty);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateNotes(string? notes)
        {
            Notes = notes;
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
        public bool IsReliable => Rating >= 4;

        public bool HasSpecialty(string specialty) => Specialties.Contains(specialty);
    }
}