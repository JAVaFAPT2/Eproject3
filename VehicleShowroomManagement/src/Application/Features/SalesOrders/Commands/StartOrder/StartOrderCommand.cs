

namespace VehicleShowroomManagement.Application.Features.SalesOrders.Commands.StartOrder
{
    /// <summary>
    /// Command to start an order - only updates status, doesn't create full order
    /// </summary>
    public record StartOrderCommand : IRequest<string>
    {
        public string CustomerId { get; init; }
        public string VehicleId { get; init; }
        public OrderStatus InitialStatus { get; init; } = OrderStatus.Pending;

        public StartOrderCommand(
            string customerId,
            string vehicleId,
            OrderStatus initialStatus = OrderStatus.Pending)
        {
            CustomerId = customerId;
            VehicleId = vehicleId;
            InitialStatus = initialStatus;
        }
    }
}
