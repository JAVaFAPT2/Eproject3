using MediatR;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.UpdateVehicleStatus
{
    /// <summary>
    /// Handler for updating vehicle status
    /// </summary>
    public class UpdateVehicleStatusCommandHandler : IRequestHandler<UpdateVehicleStatusCommand>
    {
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVehicleStatusCommandHandler(
            IRepository<Vehicle> vehicleRepository,
            IUnitOfWork unitOfWork)
        {
            _vehicleRepository = vehicleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateVehicleStatusCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);
            if (vehicle == null || vehicle.IsDeleted)
                throw new ArgumentException($"Vehicle with ID {request.VehicleId} not found");

            // Update status using domain method
            vehicle.UpdateStatus(request.Status);

            await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}