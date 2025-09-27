using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// VehicleImage entity representing images associated with vehicles
    /// Manages vehicle photographs and image optimization
    /// </summary>
    public class VehicleImage 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("vehicleId")]
        [BsonRequired]
        public string VehicleId { get; set; } = string.Empty;

        [BsonElement("imageUrl")]
        [BsonRequired]
        public string ImageUrl { get; set; } = string.Empty;

        [BsonElement("imageType")]
        [BsonRequired]
        public string ImageType { get; set; } = "Exterior"; // Exterior, Interior, Engine, Other

        [BsonElement("fileName")]
        [BsonRequired]
        public string FileName { get; set; } = string.Empty;

        [BsonElement("fileSize")]
        [BsonRequired]
        public int FileSize { get; set; } // in bytes

        [BsonElement("publicId")]
        public string PublicId { get; set; } = string.Empty; // Cloudinary public ID

        [BsonElement("originalFileName")]
        public string OriginalFileName { get; set; } = string.Empty;

        [BsonElement("contentType")]
        public string ContentType { get; set; } = string.Empty;

        [BsonElement("isPrimary")]
        public bool IsPrimary { get; set; } = false; // Primary image for vehicle

        [BsonElement("uploadedAt")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Domain Methods
        public void UpdateImage(string imageUrl, string imageType, string fileName, int fileSize)
        {
            ImageUrl = imageUrl;
            ImageType = imageType;
            FileName = fileName;
            FileSize = fileSize;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeImageType(string imageType)
        {
            ImageType = imageType;
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