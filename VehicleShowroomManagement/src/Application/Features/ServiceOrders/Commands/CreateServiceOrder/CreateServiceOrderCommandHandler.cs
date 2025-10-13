namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder
{
    public class CreateServiceOrderCommandHandler(
        IRepository<ServiceOrder> serviceOrderRepository,
        IRepository<Order> orderRepository) : IRequestHandler<CreateServiceOrderCommand, string>
    {
        public async Task<string> Handle(CreateServiceOrderCommand request, CancellationToken cancellationToken)
        {
            // Verify order exists
            _ = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken) ?? throw new InvalidOperationException("Order not found");

            var serviceOrder = new ServiceOrder(
                request.OrderId,
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

