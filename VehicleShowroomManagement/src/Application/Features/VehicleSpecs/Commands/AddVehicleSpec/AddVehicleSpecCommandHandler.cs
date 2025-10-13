namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Commands.AddVehicleSpec
{
    /// <summary>
    /// Handler for adding a specification to a vehicle
    /// </summary>
    public class AddVehicleSpecCommandHandler(
        IRepository<VehicleSpec> specRepository,
        IRepository<Vehicle> vehicleRepository) : IRequestHandler<AddVehicleSpecCommand, string>
    {

        public async Task<string> Handle(AddVehicleSpecCommand request, CancellationToken cancellationToken)
        {
            // Verify vehicle exists
            _ = await vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken) ?? throw new KeyNotFoundException($"Vehicle with ID {request.VehicleId} not found");

            // Create spec
            var spec = new VehicleSpec(
                request.VehicleId,
                request.SpecName,
                request.SpecValue,
                request.DisplayOrder,
                request.GroupName);

            await specRepository.AddAsync(spec, cancellationToken);

            return spec.Id;
        }
    }
}

