namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Commands.DeleteVehicleSpec
{
    /// <summary>
    /// Handler for deleting a vehicle specification
    /// </summary>
    public class DeleteVehicleSpecCommandHandler(IRepository<VehicleSpec> specRepository) : IRequestHandler<DeleteVehicleSpecCommand>
    {

        public async Task Handle(DeleteVehicleSpecCommand request, CancellationToken cancellationToken)
        {
            var spec = await specRepository.GetByIdAsync(request.SpecId, cancellationToken) ?? throw new KeyNotFoundException($"Spec with ID {request.SpecId} not found");
            await specRepository.DeleteAsync(spec, cancellationToken);
        }
    }
}

