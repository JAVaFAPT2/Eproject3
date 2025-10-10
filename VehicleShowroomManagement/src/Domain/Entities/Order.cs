using System;
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
        [BsonRequired]
        public string DealerId { get; private set; } = string.Empty;

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
        public OrderStatus Status { get; private set; } = OrderStatus.Waiting;

        [BsonElement("salePrice")]
        public decimal SalePrice { get; private set; }

        [BsonElement("note")]
        public string? Note { get; private set; }

        [BsonElement("reservationFrom")]
        public DateTime? ReservationFrom { get; private set; }

        [BsonElement("reservationTo")]
        public DateTime? ReservationTo { get; private set; }

        // Internal constructor for MongoDB
        internal Order() { }

        [BsonConstructor]
        public Order(string customerId, string dealerId, string modelNumber, decimal salePrice, 
            string? vehicleId = null, DateTime? appointmentDate = null, string? note = null)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));

            if (string.IsNullOrWhiteSpace(dealerId))
                throw new ArgumentException("Dealer ID cannot be null or empty", nameof(dealerId));

            if (string.IsNullOrWhiteSpace(modelNumber))
                throw new ArgumentException("Model number cannot be null or empty", nameof(modelNumber));

            if (salePrice < 0)
                throw new ArgumentException("Sale price cannot be negative", nameof(salePrice));

            CustomerId = customerId;
            DealerId = dealerId;
            ModelNumber = modelNumber;
            VehicleId = vehicleId;
            SalePrice = salePrice;
            AppointmentDate = appointmentDate;
            Note = note;
            OrderDate = DateTime.UtcNow;
            Status = string.IsNullOrEmpty(vehicleId) ? OrderStatus.Waiting : OrderStatus.Reserved;
        }

        // Domain methods
        public void AssignVehicle(string vehicleId)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be null or empty", nameof(vehicleId));

            if (Status != OrderStatus.Waiting)
                throw new InvalidOperationException("Only waiting orders can have vehicles assigned");

            VehicleId = vehicleId;
            Status = OrderStatus.Reserved;
            ReservationFrom = DateTime.UtcNow;
        }

        public void Confirm()
        {
            if (Status != OrderStatus.Reserved)
                throw new InvalidOperationException("Only reserved orders can be confirmed");

            if (string.IsNullOrEmpty(VehicleId))
                throw new InvalidOperationException("Cannot confirm order without assigned vehicle");

            Status = OrderStatus.Confirmed;
        }

        public void Complete()
        {
            if (Status != OrderStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed orders can be completed");

            Status = OrderStatus.Completed;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Completed)
                throw new InvalidOperationException("Cannot cancel a completed order");

            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Order is already cancelled");

            Status = OrderStatus.Cancelled;
        }

        public void UpdateAppointmentDate(DateTime? appointmentDate)
        {
            AppointmentDate = appointmentDate;
        }

        public void UpdateNote(string? note)
        {
            Note = note;
        }

        public void SetReservationPeriod(DateTime from, DateTime to)
        {
            if (from >= to)
                throw new ArgumentException("Reservation 'from' date must be before 'to' date");

            ReservationFrom = from;
            ReservationTo = to;
        }

        // Computed properties
        public bool IsWaiting => Status == OrderStatus.Waiting;
        public bool IsReserved => Status == OrderStatus.Reserved;
        public bool IsConfirmed => Status == OrderStatus.Confirmed;
        public bool IsCompleted => Status == OrderStatus.Completed;
        public bool IsCancelled => Status == OrderStatus.Cancelled;
        public bool HasVehicle => !string.IsNullOrEmpty(VehicleId);
    }
}

