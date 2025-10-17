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

            // If VehicleId provided, use it; otherwise pick first available by order's model number
            Vehicle? vehicle;
            if (!string.IsNullOrWhiteSpace(request.VehicleId))
            {
                var vehicles = await vehicleRepository.FindAsync(v => v.VehicleId == request.VehicleId, cancellationToken);
                vehicle = vehicles.FirstOrDefault();
            }
            else
            {
                var vehicles = await vehicleRepository.FindAsync(v => v.ModelNumber == order.ModelNumber && v.Status == VehicleStatus.Available, cancellationToken);
                vehicle = vehicles.FirstOrDefault();
                if (vehicle != null)
                {
                    request = request with { VehicleId = vehicle.VehicleId };
                }
            }

            if (vehicle is null)
            {
                throw new InvalidOperationException("Vehicle not found");
            }

            if (vehicle.Status != VehicleStatus.Available)
            {
                throw new InvalidOperationException("Vehicle is not available");
            }

            // Assign dealer
            if (!string.IsNullOrWhiteSpace(request.DealerId))
            {
                order.SetDealer(request.DealerId);
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
