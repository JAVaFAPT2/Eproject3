using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Enums;
using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Employee aggregate root representing system employees (HR, Dealers)
    /// This is separate from User for HR management purposes
    /// </summary>
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("employeeId")]
        [BsonRequired]
        public string EmployeeId { get; private set; } = string.Empty;

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; private set; } = string.Empty;

        [BsonElement("role")]
        [BsonRequired]
        public UserRole Role { get; private set; } = default!;

        [BsonElement("position")]
        public string? Position { get; private set; }

        [BsonElement("hireDate")]
        public DateTime HireDate { get; private set; } = DateTime.UtcNow;

        [BsonElement("status")]
        public string Status { get; private set; } = "Active";

        [BsonElement("userId")]
        public string? UserId { get; private set; } // Link to User aggregate

        [BsonElement("salary")]
        public decimal? Salary { get; private set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Internal constructor for MongoDB and domain services
        internal Employee() { }

        public Employee(string employeeId, string name, UserRole role, string? position = null, DateTime? hireDate = null, decimal? salary = null)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("Employee ID cannot be null or empty", nameof(employeeId));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            EmployeeId = employeeId;
            Name = name;
            Role = role;
            Position = position;
            HireDate = hireDate ?? DateTime.UtcNow;
            Salary = salary;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain methods
        public void UpdateProfile(string name, string? position)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            Name = name;
            Position = position;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeRole(UserRole newRole)
        {
            Role = newRole;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status cannot be null or empty", nameof(status));

            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateSalary(decimal? salary)
        {
            if (salary.HasValue && salary < 0)
                throw new ArgumentException("Salary cannot be negative", nameof(salary));

            Salary = salary;
            UpdatedAt = DateTime.UtcNow;
        }

        public void LinkToUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            UserId = userId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UnlinkUser()
        {
            UserId = null;
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
        public bool IsActive => Status == "Active";

        public bool IsDealer => Role == Enums.UserRole.Dealer;

        public bool IsHR => Role == Enums.UserRole.HR;

        public bool HasUserAccount => !string.IsNullOrEmpty(UserId);
    }
}

