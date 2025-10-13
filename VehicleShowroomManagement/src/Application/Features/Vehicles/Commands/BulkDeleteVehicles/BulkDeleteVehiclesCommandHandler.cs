
namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.BulkDeleteVehicles
{
    /// <summary>
    /// Handler for bulk delete vehicles command
    /// </summary>
    public class BulkDeleteVehiclesCommandHandler(IRepository<Vehicle> vehicleRepository) : IRequestHandler<BulkDeleteVehiclesCommand>
    {

        public async Task Handle(BulkDeleteVehiclesCommand request, CancellationToken cancellationToken)
        {
            foreach (var vehicleId in request.VehicleIds)
            {
                var vehicle = await vehicleRepository.GetByIdAsync(vehicleId, cancellationToken);
                if (vehicle is not null)
                {
                    await vehicleRepository.DeleteAsync(vehicleId, cancellationToken);
                }
            }
        }
    }
}
