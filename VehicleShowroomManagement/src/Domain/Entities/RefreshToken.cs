using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// RefreshToken entity for managing JWT refresh tokens
    /// </summary>
    public class RefreshToken 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("token")]
        [BsonRequired]
        public string Token { get; set; } = string.Empty;

        [BsonElement("userId")]
        [BsonRequired]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("expiresAt")]
        [BsonRequired]
        public DateTime ExpiresAt { get; set; }

        [BsonElement("isRevoked")]
        public bool IsRevoked { get; set; } = false;

        [BsonElement("revokedAt")]
        public DateTime? RevokedAt { get; set; }

        [BsonElement("revokedByIp")]
        public string? RevokedByIp { get; set; }

        [BsonElement("replacedByToken")]
        public string? ReplacedByToken { get; set; }

        [BsonElement("reasonRevoked")]
        public string? ReasonRevoked { get; set; }

        [BsonElement("createdByIp")]
        public string? CreatedByIp { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Domain Methods
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsActive => !IsRevoked && !IsExpired && !IsDeleted;

        public void Revoke(string? ipAddress = null, string? reason = null, string? replacedByToken = null)
        {
            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
            RevokedByIp = ipAddress;
            ReasonRevoked = reason;
            ReplacedByToken = replacedByToken;
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

        public static RefreshToken Create(string token, string userId, DateTime expiresAt, string? ipAddress = null)
        {
            return new RefreshToken
            {
                Token = token,
                UserId = userId,
                ExpiresAt = expiresAt,
                CreatedByIp = ipAddress,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
