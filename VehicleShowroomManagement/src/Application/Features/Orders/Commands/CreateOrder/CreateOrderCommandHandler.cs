namespace VehicleShowroomManagement.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler(
        IRepository<Order> orderRepository,
        IRepository<Vehicle> vehicleRepository,
        IRepository<VehicleModel> modelRepository) : IRequestHandler<CreateOrderCommand, string>
    {
        private readonly IRepository<Order> _orderRepository = orderRepository;
        private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
        private readonly IRepository<VehicleModel> _modelRepository = modelRepository;

        public async Task<string> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Verify model exists
            var models = await _modelRepository.FindAsync(vm => vm.ModelNumber == request.ModelNumber, cancellationToken);
            if (!models.Any())
            {
                throw new InvalidOperationException("Vehicle model not found");
            }

            // If vehicle ID provided, verify it exists and is available
            if (!string.IsNullOrEmpty(request.VehicleId))
            {
                var vehicles = await _vehicleRepository.FindAsync(v => v.VehicleId == request.VehicleId, cancellationToken);
                var vehicle = vehicles.FirstOrDefault();

                if (vehicle == null)
                {
                    throw new InvalidOperationException("Vehicle not found");
                }

                if (vehicle.Status != VehicleStatus.InStock)
                {
                    throw new InvalidOperationException("Vehicle is not available");
                }

                // Reserve the vehicle
                vehicle.Reserve();
                await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
            }

            var order = new Order(
                request.CustomerId,
                request.DealerId,
                request.ModelNumber,
                request.SalePrice,
                request.VehicleId,
                request.AppointmentDate,
                request.Note);

            await _orderRepository.AddAsync(order, cancellationToken);

            return order.Id;
        }
    }
}
