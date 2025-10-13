using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// BillingDocument entity for invoices and billing
    /// </summary>
    public class BillingDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("orderId")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string OrderId { get; private set; } = string.Empty;

        [BsonElement("createdBy")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string CreatedBy { get; private set; } = string.Empty;

        [BsonElement("billDate")]
        public DateTime BillDate { get; private set; } = DateTime.UtcNow;

        [BsonElement("appointmentDate")]
        public DateTime? AppointmentDate { get; private set; }

        [BsonElement("amount")]
        [BsonRequired]
        public decimal Amount { get; private set; }

    [BsonElement("status")]
    public BillingStatus Status { get; private set; } = BillingStatus.Unpaid;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Internal constructor for MongoDB
    internal BillingDocument() { }

        [BsonConstructor]
        public BillingDocument(string orderId, string createdBy, decimal amount, DateTime? appointmentDate = null)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                throw new ArgumentException("Order ID cannot be null or empty", nameof(orderId));

            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("CreatedBy cannot be null or empty", nameof(createdBy));

            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(amount));

            OrderId = orderId;
            CreatedBy = createdBy;
            Amount = amount;
            AppointmentDate = appointmentDate;
            BillDate = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

    // Domain methods
    public void UpdateAmount(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        Amount = amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAppointmentDate(DateTime? appointmentDate)
    {
        AppointmentDate = appointmentDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsPartiallyPaid()
    {
        if (Status == BillingStatus.Paid)
            throw new InvalidOperationException("Cannot change status of a fully paid billing document");

        Status = BillingStatus.PartiallyPaid;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsPaid()
    {
        Status = BillingStatus.Paid;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsUnpaid()
    {
        Status = BillingStatus.Unpaid;
        UpdatedAt = DateTime.UtcNow;
    }

        // Computed properties
        public bool IsUnpaid => Status == BillingStatus.Unpaid;
        public bool IsPartiallyPaid => Status == BillingStatus.PartiallyPaid;
        public bool IsPaid => Status == BillingStatus.Paid;
    }
}
