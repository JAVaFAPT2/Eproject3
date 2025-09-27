using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// WaitingList entity representing customer waiting list details
    /// Manages customer waiting list for specific vehicle models or types
    /// </summary>
    public class WaitingList 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("waitId")]
        [BsonRequired]
        public string WaitId { get; set; } = string.Empty;

        [BsonElement("customerId")]
        [BsonRequired]
        public string CustomerId { get; set; } = string.Empty;

        [BsonElement("modelNumber")]
        [BsonRequired]
        public string ModelNumber { get; set; } = string.Empty;

        [BsonElement("requestDate")]
        [BsonRequired]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [BsonElement("status")]
        public string Status { get; set; } = "Waiting"; // Waiting, Notified, Converted, Cancelled

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Domain Methods
        public void UpdateStatus(string status)
        {
            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateRequestDate(DateTime requestDate)
        {
            RequestDate = requestDate;
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
