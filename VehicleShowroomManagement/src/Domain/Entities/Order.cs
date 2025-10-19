using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Order entity for customer vehicle orders (replaces SalesOrder)
    /// </summary>
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("customerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string CustomerId { get; private set; } = string.Empty;

        [BsonElement("dealerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? DealerId { get; private set; }

        [BsonElement("modelNumber")]
        [BsonRequired]
        public string ModelNumber { get; private set; } = string.Empty;

        [BsonElement("vehicleId")]
        public string? VehicleId { get; private set; }

        [BsonElement("orderDate")]
        public DateTime OrderDate { get; private set; } = DateTime.UtcNow;

        [BsonElement("appointmentDate")]
        public DateTime? AppointmentDate { get; private set; }

        [BsonElement("status")]
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;

        [BsonElement("salePrice")]
        public decimal SalePrice { get; private set; }

        [BsonElement("note")]
        public string? Note { get; private set; }

        [BsonElement("reservationFrom")]
        public DateTime? ReservationFrom { get; private set; }

    [BsonElement("reservationTo")]
    public DateTime? ReservationTo { get; private set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Internal constructor for MongoDB
    internal Order() { }

        [BsonConstructor]
        public Order(string customerId, string modelNumber, decimal salePrice, string? dealerId = null)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));

            if (string.IsNullOrWhiteSpace(modelNumber))
                throw new ArgumentException("Model number cannot be null or empty", nameof(modelNumber));

            if (salePrice < 0)
                throw new ArgumentException("Sale price cannot be negative", nameof(salePrice));

            CustomerId = customerId;
            DealerId = dealerId;
            ModelNumber = modelNumber;
            SalePrice = salePrice;
            OrderDate = DateTime.UtcNow;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
    }

    // Domain methods
    public void AssignVehicle(string vehicleId)
    {
        if (string.IsNullOrWhiteSpace(vehicleId))
            throw new ArgumentException("Vehicle ID cannot be null or empty", nameof(vehicleId));

        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only waiting orders can have vehicles assigned");

        VehicleId = vehicleId;
        Status = OrderStatus.Confirmed;
        ReservationFrom = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Only reserved orders can be confirmed");

        if (string.IsNullOrEmpty(VehicleId))
            throw new InvalidOperationException("Cannot confirm order without assigned vehicle");

        Status = OrderStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDealer(string dealerId)
    {
        if (string.IsNullOrWhiteSpace(dealerId))
            throw new ArgumentException("Dealer ID cannot be empty", nameof(dealerId));
        DealerId = dealerId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateNote(string? note)
    {
        Note = note;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Order is already completed");
        
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Cannot complete a cancelled order");

        Status = OrderStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed order");

        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Order is already cancelled");

        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ClearVehicle()
    {
        VehicleId = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(OrderStatus status)
    {
        switch (status)
        {
            case OrderStatus.Pending:
                // Keep as pending if already pending; no-op otherwise
                if (Status == OrderStatus.Pending)
                {
                    UpdatedAt = DateTime.UtcNow;
                }
                break;
            case OrderStatus.Confirmed:
                Confirm();
                break;
            case OrderStatus.Completed:
                Complete();
                break;
            case OrderStatus.Cancelled:
                Cancel();
                break;
            default:
                throw new InvalidOperationException("Unsupported status transition");
        }
    }


    public void UpdateAppointmentDate(DateTime? appointmentDate)
    {
        AppointmentDate = appointmentDate;
        UpdatedAt = DateTime.UtcNow;
    }


    public void SetReservationPeriod(DateTime from, DateTime to)
    {
        if (from >= to)
            throw new ArgumentException("Reservation 'from' date must be before 'to' date");

        ReservationFrom = from;
        ReservationTo = to;
        UpdatedAt = DateTime.UtcNow;
    }

        // Computed properties
        public bool IsPending => Status == OrderStatus.Pending;
        public bool IsConfirmed => Status == OrderStatus.Confirmed;
        public bool IsCompleted => Status == OrderStatus.Completed;
        public bool IsCancelled => Status == OrderStatus.Cancelled;
        public bool HasVehicle => !string.IsNullOrEmpty(VehicleId);
    }
}

