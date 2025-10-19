namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder
{
    public class CreateServiceOrderCommandHandler(
        IRepository<ServiceOrder> serviceOrderRepository,
        IRepository<Order> orderRepository) : IRequestHandler<CreateServiceOrderCommand, string>
    {
        public async Task<string> Handle(CreateServiceOrderCommand request, CancellationToken cancellationToken)
        {
            // Validate input parameters first
            if (string.IsNullOrWhiteSpace(request.OrderId))
                throw new ArgumentException("Order ID cannot be null or empty", nameof(request.OrderId));
            
            if (string.IsNullOrWhiteSpace(request.CustomerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(request.CustomerId));
            
            if (string.IsNullOrWhiteSpace(request.CreatedBy))
                throw new ArgumentException("Created by cannot be null or empty", nameof(request.CreatedBy));
            
            if (request.Cost < 0)
                throw new ArgumentException("Cost cannot be negative", nameof(request.Cost));

            if (request.AppointmentDate.HasValue && request.AppointmentDate.Value < DateTime.Now)
                throw new ArgumentException("Appointment date cannot be in the past", nameof(request.AppointmentDate));

            // Verify order exists
            _ = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken) ?? throw new InvalidOperationException("Order not found");

            var serviceOrder = new ServiceOrder(
                request.OrderId,
                request.CustomerId,
                request.CreatedBy,
                request.Type,
                request.Cost,
                request.AppointmentDate,
                request.Description);

            await serviceOrderRepository.AddAsync(serviceOrder, cancellationToken);

            return serviceOrder.Id;
        }
    }
}

