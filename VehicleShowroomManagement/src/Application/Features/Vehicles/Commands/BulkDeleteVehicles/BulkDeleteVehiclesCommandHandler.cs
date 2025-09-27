using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.BulkDeleteVehicles
{
    /// <summary>
    /// Handler for bulk delete vehicles command
    /// </summary>
    public class BulkDeleteVehiclesCommandHandler : IRequestHandler<BulkDeleteVehiclesCommand>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public BulkDeleteVehiclesCommandHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task Handle(BulkDeleteVehiclesCommand request, CancellationToken cancellationToken)
        {
            foreach (var vehicleId in request.VehicleIds)
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
                if (vehicle != null)
                {
                    await _vehicleRepository.DeleteAsync(vehicleId);
                }
            }
        }
    }
}