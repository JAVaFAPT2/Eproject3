using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// OrderNote entity for tracking order notes and communications
    /// </summary>
    public class OrderNote
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("orderId")]
        [BsonRequired]
        public string OrderId { get; private set; } = string.Empty;

        [BsonElement("orderType")]
        [BsonRequired]
        public string OrderType { get; private set; } = "Sales"; // Sales, Service, Purchase

        [BsonElement("noteType")]
        [BsonRequired]
        public string NoteType { get; private set; } = "General"; // General, Internal, Customer, Delivery, Technical

        [BsonElement("content")]
        [BsonRequired]
        public string Content { get; private set; } = string.Empty;

        [BsonElement("addedBy")]
        [BsonRequired]
        public string AddedBy { get; private set; } = string.Empty;

        [BsonElement("isInternal")]
        public bool IsInternal { get; private set; } = false;

        [BsonElement("isCustomerVisible")]
        public bool IsCustomerVisible { get; private set; } = true;

        [BsonElement("priority")]
        public int Priority { get; private set; } = 1; // 1=Low, 2=Medium, 3=High

        [BsonElement("scheduledDate")]
        public DateTime? ScheduledDate { get; private set; }

        [BsonElement("attachments")]
        public List<string> Attachments { get; private set; } = new List<string>();

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Private constructor for MongoDB
        private OrderNote() { }

        public OrderNote(
            string orderId,
            string orderType,
            string noteType,
            string content,
            string addedBy,
            bool isInternal = false,
            bool isCustomerVisible = true,
            int priority = 1,
            DateTime? scheduledDate = null)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                throw new ArgumentException("Order ID cannot be null or empty", nameof(orderId));

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Content cannot be null or empty", nameof(content));

            if (string.IsNullOrWhiteSpace(addedBy))
                throw new ArgumentException("Added by cannot be null or empty", nameof(addedBy));

            OrderId = orderId;
            OrderType = orderType;
            NoteType = noteType;
            Content = content;
            AddedBy = addedBy;
            IsInternal = isInternal;
            IsCustomerVisible = isCustomerVisible;
            Priority = priority;
            ScheduledDate = scheduledDate;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain methods
        public void UpdateContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("Content cannot be null or empty", nameof(newContent));

            Content = newContent;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPriority(int priority)
        {
            if (priority < 1 || priority > 3)
                throw new ArgumentException("Priority must be between 1 and 3", nameof(priority));

            Priority = priority;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddAttachment(string attachmentUrl)
        {
            if (string.IsNullOrWhiteSpace(attachmentUrl))
                throw new ArgumentException("Attachment URL cannot be null or empty", nameof(attachmentUrl));

            Attachments.Add(attachmentUrl);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveAttachment(string attachmentUrl)
        {
            Attachments.Remove(attachmentUrl);
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsInternal()
        {
            IsInternal = true;
            IsCustomerVisible = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsCustomerVisible()
        {
            IsCustomerVisible = true;
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
        public bool IsHighPriority => Priority == 3;
        public bool IsScheduled => ScheduledDate.HasValue;
        public bool HasAttachments => Attachments.Any();
    }
}