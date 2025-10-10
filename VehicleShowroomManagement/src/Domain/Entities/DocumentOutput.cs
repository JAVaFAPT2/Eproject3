using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// DocumentOutput entity for generated documents (invoices, data sheets, confirmations)
    /// </summary>
    public class DocumentOutput
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("entityType")]
        [BsonRequired]
        public EntityType EntityType { get; private set; }

        [BsonElement("entityId")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string EntityId { get; private set; } = string.Empty;

        [BsonElement("fileType")]
        [BsonRequired]
        public FileType FileType { get; private set; }

        [BsonElement("fileUrl")]
        [BsonRequired]
        public string FileUrl { get; private set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Internal constructor for MongoDB
        internal DocumentOutput() { }

        [BsonConstructor]
        public DocumentOutput(EntityType entityType, string entityId, FileType fileType, string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(entityId))
                throw new ArgumentException("Entity ID cannot be null or empty", nameof(entityId));

            if (string.IsNullOrWhiteSpace(fileUrl))
                throw new ArgumentException("File URL cannot be null or empty", nameof(fileUrl));

            EntityType = entityType;
            EntityId = entityId;
            FileType = fileType;
            FileUrl = fileUrl;
            CreatedAt = DateTime.UtcNow;
        }

        // Domain methods
        public void UpdateFileUrl(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                throw new ArgumentException("File URL cannot be null or empty", nameof(fileUrl));

            FileUrl = fileUrl;
        }
    }
}

