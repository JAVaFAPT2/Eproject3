namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.CreateVehicle
{
    /// <summary>
    /// Handler for creating a new vehicle
    /// </summary>
    public class CreateVehicleCommandHandler(IRepository<Vehicle> vehicleRepository) : IRequestHandler<CreateVehicleCommand, string>
    {

        public async Task<string> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            // Validate input parameters first
            if (string.IsNullOrWhiteSpace(request.VehicleId))
                throw new ArgumentException("Vehicle ID cannot be null or empty", nameof(request.VehicleId));
            
            if (string.IsNullOrWhiteSpace(request.ModelNumber))
                throw new ArgumentException("Model number cannot be null or empty", nameof(request.ModelNumber));
            
            if (request.PurchasePrice < 0)
                throw new ArgumentException("Purchase price cannot be negative", nameof(request.PurchasePrice));
            
            if (string.IsNullOrWhiteSpace(request.Vin))
                throw new ArgumentException("VIN cannot be null or empty", nameof(request.Vin));
            
            if (string.IsNullOrWhiteSpace(request.ExternalNumber))
                throw new ArgumentException("External number cannot be null or empty", nameof(request.ExternalNumber));

            // Create vehicle with new schema
            var vehicle = new Vehicle(
                request.VehicleId,
                request.ModelNumber,
                request.PurchasePrice,
                request.ExternalNumber,
                request.Vin);

            // Add to repository
            await vehicleRepository.AddAsync(vehicle, cancellationToken);

            return vehicle.VehicleId;
        }
    }
}
