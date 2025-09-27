using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Enums;

using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// SalesOrder aggregate root representing customer sales orders
    /// </summary>
    public class SalesOrder 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("orderNumber")]
        [BsonRequired]
        public string OrderNumber { get; private set; } = string.Empty;

        [BsonElement("customerId")]
        [BsonRequired]
        public string CustomerId { get; private set; } = string.Empty;

        [BsonElement("vehicleId")]
        [BsonRequired]
        public string VehicleId { get; private set; } = string.Empty;

        [BsonElement("salesPersonId")]
        [BsonRequired]
        public string SalesPersonId { get; private set; } = string.Empty;

        [BsonElement("orderDate")]
        public DateTime OrderDate { get; private set; } = DateTime.UtcNow;

        [BsonElement("totalAmount")]
        public decimal TotalAmount { get; private set; }

        [BsonElement("paymentMethod")]
        public PaymentMethod PaymentMethod { get; private set; }

        [BsonElement("status")]
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;

        [BsonElement("dataSheetOutput")]
        public DocumentOutput? DataSheetOutput { get; private set; }

        [BsonElement("confirmationOutput")]
        public DocumentOutput? ConfirmationOutput { get; private set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Private constructor for MongoDB
        private SalesOrder() { }

        public SalesOrder(string orderNumber, string customerId, string vehicleId, string salesPersonId, decimal totalAmount, PaymentMethod paymentMethod)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
                throw new ArgumentException("Order number cannot be null or empty", nameof(orderNumber));

            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));

            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be null or empty", nameof(vehicleId));

            if (string.IsNullOrWhiteSpace(salesPersonId))
                throw new ArgumentException("Sales person ID cannot be null or empty", nameof(salesPersonId));

            if (totalAmount < 0)
                throw new ArgumentException("Total amount cannot be negative", nameof(totalAmount));

            OrderNumber = orderNumber;
            CustomerId = customerId;
            VehicleId = vehicleId;
            SalesPersonId = salesPersonId;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
            OrderDate = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain methods
        public void UpdateStatus(OrderStatus status)
        {
            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateTotalAmount(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Total amount cannot be negative", nameof(amount));

            TotalAmount = amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            PaymentMethod = paymentMethod;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Confirm()
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Only pending orders can be confirmed");

            Status = OrderStatus.Confirmed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Process()
        {
            if (Status != OrderStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed orders can be processed");

            Status = OrderStatus.Processing;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            if (Status != OrderStatus.Processing)
                throw new InvalidOperationException("Only processing orders can be completed");

            Status = OrderStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Completed)
                throw new InvalidOperationException("Completed orders cannot be cancelled");

            Status = OrderStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }

        public void GenerateDataSheet(string format, string url)
        {
            if (string.IsNullOrWhiteSpace(format))
                throw new ArgumentException("Format cannot be null or empty", nameof(format));

            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty", nameof(url));

            DataSheetOutput = new DocumentOutput
            {
                Format = format,
                GeneratedAt = DateTime.UtcNow,
                Url = url
            };
            UpdatedAt = DateTime.UtcNow;
        }

        public void GenerateConfirmation(string format, string url)
        {
            if (string.IsNullOrWhiteSpace(format))
                throw new ArgumentException("Format cannot be null or empty", nameof(format));

            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty", nameof(url));

            ConfirmationOutput = new DocumentOutput
            {
                Format = format,
                GeneratedAt = DateTime.UtcNow,
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

        // Computed properties
        public bool IsPending => Status == OrderStatus.Pending;
        public bool IsConfirmed => Status == OrderStatus.Confirmed;
        public bool IsProcessing => Status == OrderStatus.Processing;
        public bool IsCompleted => Status == OrderStatus.Completed;
        public bool IsCancelled => Status == OrderStatus.Cancelled;
        public bool IsRefunded => Status == OrderStatus.Refunded;
        public bool CanBeModified => Status == OrderStatus.Pending || Status == OrderStatus.Confirmed;
    }

    /// <summary>
    /// Document output information
    /// </summary>
    public class DocumentOutput
    {
        [BsonElement("format")]
        public string Format { get; set; } = string.Empty;

        [BsonElement("generatedAt")]
        public DateTime GeneratedAt { get; set; }

        [BsonElement("url")]
        public string Url { get; set; } = string.Empty;
    }
}