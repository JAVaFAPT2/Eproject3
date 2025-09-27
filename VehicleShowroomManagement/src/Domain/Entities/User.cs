using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Enums;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// User aggregate root for authentication and authorization
    /// </summary>
    public class User : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("username")]
        [BsonRequired]
        public string Username { get; private set; } = string.Empty;

        [BsonElement("email")]
        [BsonRequired]
        public string Email { get; private set; } = string.Empty;

        [BsonElement("passwordHash")]
        [BsonRequired]
        public string PasswordHash { get; private set; } = string.Empty;

        [BsonElement("firstName")]
        [BsonRequired]
        public string FirstName { get; private set; } = string.Empty;

        [BsonElement("lastName")]
        [BsonRequired]
        public string LastName { get; private set; } = string.Empty;

        [BsonElement("phone")]
        public string? Phone { get; private set; }

        [BsonElement("role")]
        [BsonRequired]
        public UserRole Role { get; private set; }

        [BsonElement("isActive")]
        public bool IsActive { get; private set; } = true;

        [BsonElement("emailConfirmed")]
        public bool EmailConfirmed { get; private set; } = false;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        [BsonElement("refreshTokens")]
        public List<RefreshToken> RefreshTokens { get; private set; } = new();

        // Private constructor for MongoDB
        private User() { }

        public User(string username, Email email, string passwordHash, string firstName, string lastName, UserRole role, string? phone = null)
        {
            Username = username;
            Email = email.Value;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Phone = phone;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain methods
        public void UpdateProfile(string firstName, string lastName, string? phone)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be null or empty", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));

            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password hash cannot be null or empty", nameof(newPasswordHash));

            PasswordHash = newPasswordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeRole(UserRole newRole)
        {
            Role = newRole;
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

        public void ConfirmEmail()
        {
            EmailConfirmed = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddRefreshToken(RefreshToken refreshToken)
        {
            RefreshTokens.Add(refreshToken);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RevokeRefreshToken(string tokenId)
        {
            var token = RefreshTokens.FirstOrDefault(t => t.Id == tokenId);
            if (token != null)
            {
                token.Revoke();
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void RevokeAllRefreshTokens()
        {
            foreach (var token in RefreshTokens)
            {
                token.Revoke();
            }
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            RevokeAllRefreshTokens();
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }

        // Computed properties
        public string FullName => $"{FirstName} {LastName}";

        public bool IsAdmin => Role == UserRole.Admin;

        public bool IsHR => Role == UserRole.HR;

        public bool IsDealer => Role == UserRole.Dealer;

        public bool HasValidRefreshToken => RefreshTokens.Any(t => t.IsActive && !t.IsRevoked);
    }
}