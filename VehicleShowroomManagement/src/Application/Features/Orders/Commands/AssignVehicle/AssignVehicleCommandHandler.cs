using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.Orders.Commands.AssignVehicle
{
    public class AssignVehicleCommandHandler : IRequestHandler<AssignVehicleCommand, bool>
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public AssignVehicleCommandHandler(
            IRepository<Order> orderRepository,
            IRepository<Vehicle> vehicleRepository)
        {
            _orderRepository = orderRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<bool> Handle(AssignVehicleCommand request, CancellationToken cancellationToken)
        {
            // Get order
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }

            // Verify vehicle exists and is available
            var vehicles = await _vehicleRepository.FindAsync(v => v.VehicleId == request.VehicleId);
            var vehicle = vehicles.FirstOrDefault();

            if (vehicle == null)
            {
                throw new InvalidOperationException("Vehicle not found");
            }

            if (vehicle.Status != VehicleStatus.InStock)
            {
                throw new InvalidOperationException("Vehicle is not available");
            }

            // Assign vehicle to order
            order.AssignVehicle(request.VehicleId);
            await _orderRepository.UpdateAsync(order);

            // Reserve vehicle
            vehicle.Reserve();
            await _vehicleRepository.UpdateAsync(vehicle);

            return true;
        }
    }
}
