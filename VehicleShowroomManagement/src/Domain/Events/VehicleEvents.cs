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
        public string SalesOrderId { get; init; }
        public string CustomerId { get; init; }
        public decimal SalePrice { get; init; }

        public VehicleSoldEvent(string vehicleId, string salesOrderId, string customerId, decimal salePrice)
        {
            VehicleId = vehicleId;
            SalesOrderId = salesOrderId;
            CustomerId = customerId;
            SalePrice = salePrice;
        }
    }

    /// <summary>
    /// Vehicle registration created domain event
    /// </summary>
    public record VehicleRegistrationCreatedEvent : DomainEvent
    {
        public string VehicleRegistrationId { get; init; }
        public string RegistrationNumber { get; init; }
        public string VehicleId { get; init; }

        public VehicleRegistrationCreatedEvent(string vehicleRegistrationId, string registrationNumber)
        {
            VehicleRegistrationId = vehicleRegistrationId;
            RegistrationNumber = registrationNumber;
        }
    }

    /// <summary>
    /// Vehicle image created domain event
    /// </summary>
    public record VehicleImageCreatedEvent : DomainEvent
    {
        public string VehicleImageId { get; init; }
        public string VehicleId { get; init; }
        public string ImageType { get; init; }
        public bool IsPrimary { get; init; }

        public VehicleImageCreatedEvent(string vehicleImageId, string vehicleId)
        {
            VehicleImageId = vehicleImageId;
            VehicleId = vehicleId;
        }
    }
}