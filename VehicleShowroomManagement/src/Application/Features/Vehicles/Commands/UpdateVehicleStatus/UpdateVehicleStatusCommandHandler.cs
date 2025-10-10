using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.UpdateVehicleStatus
{
    /// <summary>
    /// Handler for updating vehicle status
    /// </summary>
    public class UpdateVehicleStatusCommandHandler : IRequestHandler<UpdateVehicleStatusCommand>
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public UpdateVehicleStatusCommandHandler(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task Handle(UpdateVehicleStatusCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
                throw new ArgumentException($"Vehicle with ID {request.VehicleId} not found");

            // Update status using domain method
            vehicle.UpdateStatus(request.Status);

            await _vehicleRepository.UpdateAsync(vehicle);
        }
    }
}
