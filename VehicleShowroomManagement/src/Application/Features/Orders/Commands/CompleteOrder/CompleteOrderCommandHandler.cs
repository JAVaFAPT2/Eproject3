using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Orders.Commands.CompleteOrder
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommand, bool>
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public CompleteOrderCommandHandler(
            IRepository<Order> orderRepository,
            IRepository<Vehicle> vehicleRepository)
        {
            _orderRepository = orderRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<bool> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }

            order.Complete();
            await _orderRepository.UpdateAsync(order);

            // Mark vehicle as sold
            if (!string.IsNullOrEmpty(order.VehicleId))
            {
                var vehicles = await _vehicleRepository.FindAsync(v => v.VehicleId == order.VehicleId);
                var vehicle = vehicles.FirstOrDefault();

                if (vehicle != null)
                {
                    vehicle.Sell();
                    await _vehicleRepository.UpdateAsync(vehicle);
                }
            }

            return true;
        }
    }
}
