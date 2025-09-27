using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Domain.Events
{
    /// <summary>
    /// Order created domain event
    /// </summary>
    public record OrderCreatedEvent : DomainEvent
    {
        public string OrderId { get; init; }
        public string OrderNumber { get; init; }
        public string CustomerId { get; init; }
        public string VehicleId { get; init; }
        public decimal TotalAmount { get; init; }

        public OrderCreatedEvent(string orderId, string orderNumber, string customerId, string vehicleId, decimal totalAmount)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
            CustomerId = customerId;
            VehicleId = vehicleId;
            TotalAmount = totalAmount;
        }
    }

    /// <summary>
    /// Order status changed domain event
    /// </summary>
    public record OrderStatusChangedEvent : DomainEvent
    {
        public string OrderId { get; init; }
        public OrderStatus OldStatus { get; init; }
        public OrderStatus NewStatus { get; init; }

        public OrderStatusChangedEvent(string orderId, OrderStatus oldStatus, OrderStatus newStatus)
        {
            OrderId = orderId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }

    /// <summary>
    /// Order completed domain event
    /// </summary>
    public record OrderCompletedEvent : DomainEvent
    {
        public string OrderId { get; init; }
        public string OrderNumber { get; init; }
        public string CustomerId { get; init; }
        public string VehicleId { get; init; }
        public decimal TotalAmount { get; init; }

        public OrderCompletedEvent(string orderId, string orderNumber, string customerId, string vehicleId, decimal totalAmount)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
            CustomerId = customerId;
            VehicleId = vehicleId;
            TotalAmount = totalAmount;
        }
    }

    /// <summary>
    /// Purchase order created domain event
    /// </summary>
    public record PurchaseOrderCreatedEvent : DomainEvent
    {
        public string PurchaseOrderId { get; init; }
        public string OrderNumber { get; init; }
        public string ModelNumber { get; init; }
        public decimal TotalAmount { get; init; }

        public PurchaseOrderCreatedEvent(string purchaseOrderId, string orderNumber)
        {
            PurchaseOrderId = purchaseOrderId;
            OrderNumber = orderNumber;
        }
    }

    /// <summary>
    /// Service order created domain event
    /// </summary>
    public record ServiceOrderCreatedEvent : DomainEvent
    {
        public string ServiceOrderId { get; init; }
        public string ServiceOrderNumber { get; init; }
        public string SalesOrderId { get; init; }
        public decimal Cost { get; init; }

        public ServiceOrderCreatedEvent(string serviceOrderId, string serviceOrderNumber)
        {
            ServiceOrderId = serviceOrderId;
            ServiceOrderNumber = serviceOrderNumber;
        }
    }

    /// <summary>
    /// Billing document created domain event
    /// </summary>
    public record BillingDocumentCreatedEvent : DomainEvent
    {
        public string BillingDocumentId { get; init; }
        public string BillNumber { get; init; }
        public string SalesOrderId { get; init; }
        public decimal Amount { get; init; }

        public BillingDocumentCreatedEvent(string billingDocumentId, string billNumber)
        {
            BillingDocumentId = billingDocumentId;
            BillNumber = billNumber;
        }
    }
}