namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Commands.AddVehicleSpec
{
    /// <summary>
    /// Handler for adding a specification to a vehicle
    /// </summary>
    public class AddVehicleSpecCommandHandler(
        IRepository<VehicleSpec> specRepository,
        IRepository<VehicleModel> modelRepository) : IRequestHandler<AddVehicleSpecCommand, string>
    {

        public async Task<string> Handle(AddVehicleSpecCommand request, CancellationToken cancellationToken)
        {
            // Verify level-2 model exists
            var model = await modelRepository.GetByIdAsync(request.ModelId, cancellationToken)
                ?? throw new KeyNotFoundException($"VehicleModel with number {request.ModelId} not found");
            if (model.Level != 2)
                throw new InvalidOperationException("Specifications must be attached to a level-2 model");

            // Create spec
            var spec = new VehicleSpec(
                request.ModelId,
                request.SpecName,
                request.SpecValue,
                request.DisplayOrder,
                request.GroupName);

            await specRepository.AddAsync(spec, cancellationToken);

            return spec.Id;
        }
    }
}

