namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.CreateVehicle
{
    /// <summary>
    /// Handler for creating a new vehicle
    /// </summary>
    public class CreateVehicleCommandHandler(IRepository<Vehicle> vehicleRepository)
      : IRequestHandler<CreateVehicleCommand, string>
    {
        public async Task<string> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            // ✅ Generate ID if null or empty
            var vehicleId = string.IsNullOrWhiteSpace(request.VehicleId)
                ? $"VEH-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}"
                : request.VehicleId;

            // Validate other fields
            if (string.IsNullOrWhiteSpace(request.ModelNumber))
                throw new ArgumentException("Model number cannot be null or empty", nameof(request.ModelNumber));

            if (request.PurchasePrice < 0)
                throw new ArgumentException("Purchase price cannot be negative", nameof(request.PurchasePrice));

            if (string.IsNullOrWhiteSpace(request.Vin))
                throw new ArgumentException("VIN cannot be null or empty", nameof(request.Vin));

            if (string.IsNullOrWhiteSpace(request.ExternalNumber))
                throw new ArgumentException("External number cannot be null or empty", nameof(request.ExternalNumber));

            // ✅ Create new Vehicle with guaranteed ID
            var vehicle = new Vehicle(
                vehicleId,
                request.ModelNumber,
                request.PurchasePrice,
                request.ExternalNumber,
                request.Vin);

            await vehicleRepository.AddAsync(vehicle, cancellationToken);
            return vehicle.VehicleId;
        }
    }

}
