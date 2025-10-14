namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.CreateVehicle
{
    /// <summary>
    /// Handler for creating a new vehicle
    /// </summary>
    public class CreateVehicleCommandHandler(IRepository<Vehicle> vehicleRepository) : IRequestHandler<CreateVehicleCommand, string>
    {

        public async Task<string> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            // Create vehicle with new schema
            var vehicle = new Vehicle(
                request.VehicleId,
                request.ModelNumber,
                request.PurchasePrice,
                request.ExternalNumber);

            // Add to repository
            await vehicleRepository.AddAsync(vehicle, cancellationToken);

            return vehicle.VehicleId;
        }
    }
}
