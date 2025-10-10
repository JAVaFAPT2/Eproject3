using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder
{
    public class CreateServiceOrderCommandHandler : IRequestHandler<CreateServiceOrderCommand, string>
    {
        private readonly IRepository<ServiceOrder> _serviceOrderRepository;
        private readonly IRepository<Order> _orderRepository;

        public CreateServiceOrderCommandHandler(
            IRepository<ServiceOrder> serviceOrderRepository,
            IRepository<Order> orderRepository)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _orderRepository = orderRepository;
        }

        public async Task<string> Handle(CreateServiceOrderCommand request, CancellationToken cancellationToken)
        {
            // Verify order exists
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }

            var serviceOrder = new ServiceOrder(
                request.OrderId,
                request.CreatedBy,
                request.Type,
                request.Cost,
                request.AppointmentDate,
                request.Description);

            await _serviceOrderRepository.AddAsync(serviceOrder);

            return serviceOrder.Id;
        }
    }
}

