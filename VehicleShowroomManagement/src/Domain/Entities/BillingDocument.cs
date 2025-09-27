using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Billing document entity representing invoices and bills
    /// </summary>
    public class BillingDocument : IEntity, IAuditableEntity, ISoftDelete
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("billId")]
        [BsonRequired]
        public string BillId { get; set; } = string.Empty;

        [BsonElement("salesOrderId")]
        [BsonRequired]
        public string SalesOrderId { get; set; } = string.Empty;

        [BsonElement("employeeId")]
        [BsonRequired]
        public string EmployeeId { get; set; } = string.Empty;

        [BsonElement("billDate")]
        [BsonRequired]
        public DateTime BillDate { get; set; }

        [BsonElement("amount")]
        [BsonRequired]
        public decimal Amount { get; set; }

        [BsonElement("output")]
        public BillOutput? Output { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Domain Methods
        public void UpdateAmount(decimal amount)
        {
            Amount = amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetOutput(string format, DateTime generatedAt, string url)
        {
            Output = new BillOutput
            {
                Format = format,
                GeneratedAt = generatedAt,
                Url = url
            };
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
    /// Bill output information embedded in BillingDocument entity
    /// </summary>
    public class BillOutput
    {
        [BsonElement("format")]
        [BsonRequired]
        public string Format { get; set; } = string.Empty;

        [BsonElement("generatedAt")]
        [BsonRequired]
        public DateTime GeneratedAt { get; set; }

        [BsonElement("url")]
        [BsonRequired]
        public string Url { get; set; } = string.Empty;
    }
}

