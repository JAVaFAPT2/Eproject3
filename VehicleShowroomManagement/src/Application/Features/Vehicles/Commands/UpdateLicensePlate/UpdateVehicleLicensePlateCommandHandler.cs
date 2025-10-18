using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.UpdateLicensePlate
{
    /// <summary>
    /// Handler for updating vehicle license plate
    /// </summary>
    public class UpdateVehicleLicensePlateCommandHandler(
        IRepository<Vehicle> vehicleRepository) : IRequestHandler<UpdateVehicleLicensePlateCommand, bool>
    {
        public async Task<bool> Handle(UpdateVehicleLicensePlateCommand request, CancellationToken cancellationToken)
        {
            // Find vehicle by VehicleId
            var vehicles = await vehicleRepository.FindAsync(v => v.VehicleId == request.VehicleId, cancellationToken);
            var vehicle = vehicles.FirstOrDefault();
            
            if (vehicle == null)
            {
                throw new InvalidOperationException("Vehicle not found");
            }

            // Update license plate using domain method
            vehicle.SetLicensePlate(request.LicensePlate);
            await vehicleRepository.UpdateAsync(vehicle, cancellationToken);

            return true;
        }
    }
}
