using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, string>
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<VehicleModel> _modelRepository;

        public CreateOrderCommandHandler(
            IRepository<Order> orderRepository,
            IRepository<Vehicle> vehicleRepository,
            IRepository<VehicleModel> modelRepository)
        {
            _orderRepository = orderRepository;
            _vehicleRepository = vehicleRepository;
            _modelRepository = modelRepository;
        }

        public async Task<string> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Verify model exists
            var models = await _modelRepository.FindAsync(vm => vm.ModelNumber == request.ModelNumber);
            if (!models.Any())
            {
                throw new InvalidOperationException("Vehicle model not found");
            }

            // If vehicle ID provided, verify it exists and is available
            if (!string.IsNullOrEmpty(request.VehicleId))
            {
                var vehicles = await _vehicleRepository.FindAsync(v => v.VehicleId == request.VehicleId);
                var vehicle = vehicles.FirstOrDefault();

                if (vehicle == null)
                {
                    throw new InvalidOperationException("Vehicle not found");
                }

                if (vehicle.Status != Domain.Enums.VehicleStatus.InStock)
                {
                    throw new InvalidOperationException("Vehicle is not available");
                }

                // Reserve the vehicle
                vehicle.Reserve();
                await _vehicleRepository.UpdateAsync(vehicle);
            }

            var order = new Order(
                request.CustomerId,
                request.DealerId,
                request.ModelNumber,
                request.SalePrice,
                request.VehicleId,
                request.AppointmentDate,
                request.Note);

            await _orderRepository.AddAsync(order);

            return order.Id;
        }
    }
}
