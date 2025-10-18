namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.UpdateVehicleStatus
{
    /// <summary>
    /// Handler for updating vehicle status
    /// </summary>
    public class UpdateVehicleStatusCommandHandler(IRepository<Vehicle> vehicleRepository) : IRequestHandler<UpdateVehicleStatusCommand, bool>
    {
        public async Task<bool> Handle(UpdateVehicleStatusCommand request, CancellationToken cancellationToken)
        {
            var vehicles = await vehicleRepository.FindAsync(v => v.VehicleId == request.VehicleId, cancellationToken);
            var vehicle = vehicles.FirstOrDefault();
            
            if (vehicle == null)
                throw new ArgumentException("Vehicle not found");

            vehicle.UpdateStatus(request.Status);
            await vehicleRepository.UpdateAsync(vehicle, cancellationToken);
            
            return true;
        }
    }
}