using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.CreateVehicle
{
    /// <summary>
    /// Handler for creating a new vehicle
    /// </summary>
    public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, string>
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public CreateVehicleCommandHandler(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<string> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            // Create vehicle with new schema
            var vehicle = new Vehicle(
                request.VehicleId,
                request.ModelNumber,
                request.PurchasePrice,
                request.ExternalNumber,
                request.ReceiptDate);

            // Add to repository
            await _vehicleRepository.AddAsync(vehicle);

            return vehicle.VehicleId;
        }
    }
}
