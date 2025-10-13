namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.DeleteVehicle
{
    /// <summary>
    /// Handler for delete vehicle command
    /// </summary>
    public class DeleteVehicleCommandHandler(IRepository<Vehicle> vehicleRepository) : IRequestHandler<DeleteVehicleCommand>
    {

        public async Task Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new ArgumentException("Vehicle not found");
            await vehicleRepository.DeleteAsync(vehicle, cancellationToken);
        }
    }
}
