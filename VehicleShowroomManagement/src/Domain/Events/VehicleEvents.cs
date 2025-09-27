using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Domain.Events
{
    /// <summary>
    /// Vehicle created domain event
    /// </summary>
    public record VehicleCreatedEvent : DomainEvent
    {
        public string VehicleId { get; init; }
        public string ModelNumber { get; init; }
        public decimal PurchasePrice { get; init; }

        public VehicleCreatedEvent(string vehicleId, string modelNumber, decimal purchasePrice)
        {
            VehicleId = vehicleId;
            ModelNumber = modelNumber;
            PurchasePrice = purchasePrice;
        }
    }

    /// <summary>
    /// Vehicle status changed domain event
    /// </summary>
    public record VehicleStatusChangedEvent : DomainEvent
    {
        public string VehicleId { get; init; }
        public VehicleStatus OldStatus { get; init; }
        public VehicleStatus NewStatus { get; init; }

        public VehicleStatusChangedEvent(string vehicleId, VehicleStatus oldStatus, VehicleStatus newStatus)
        {
            VehicleId = vehicleId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }

    /// <summary>
    /// Vehicle sold domain event
    /// </summary>
    public record VehicleSoldEvent : DomainEvent
    {
        public string VehicleId { get; init; }
        public string CustomerId { get; init; }
        public string SalesPersonId { get; init; }
        public decimal SalePrice { get; init; }

        public VehicleSoldEvent(string vehicleId, string customerId, string salesPersonId, decimal salePrice)
        {
            VehicleId = vehicleId;
            CustomerId = customerId;
            SalesPersonId = salesPersonId;
            SalePrice = salePrice;
        }
    }
}