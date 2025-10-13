namespace VehicleShowroomManagement.Application.Features.Orders.Commands.AssignVehicle
{
    public class AssignVehicleCommandHandler(
        IRepository<Order> orderRepository,
        IRepository<Vehicle> vehicleRepository) : IRequestHandler<AssignVehicleCommand, bool>
    {

        public async Task<bool> Handle(AssignVehicleCommand request, CancellationToken cancellationToken)
        {
            // Get order
            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order is null)
            {
                throw new InvalidOperationException("Order not found");
            }

            // Verify vehicle exists and is available
            var vehicles = await vehicleRepository.FindAsync(v => v.VehicleId == request.VehicleId, cancellationToken);
            var vehicle = vehicles.FirstOrDefault();

            if (vehicle is null)
            {
                throw new InvalidOperationException("Vehicle not found");
            }

            if (vehicle.Status != VehicleStatus.InStock)
            {
                throw new InvalidOperationException("Vehicle is not available");
            }

            // Assign vehicle to order
            order.AssignVehicle(request.VehicleId);
            await orderRepository.UpdateAsync(order, cancellationToken);

            // Reserve vehicle
            vehicle.Reserve();
            await vehicleRepository.UpdateAsync(vehicle, cancellationToken);

            return true;
        }
    }
}
