using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.BulkDeleteVehicles
{
    /// <summary>
    /// Handler for bulk delete vehicles command
    /// </summary>
    public class BulkDeleteVehiclesCommandHandler : IRequestHandler<BulkDeleteVehiclesCommand>
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public BulkDeleteVehiclesCommandHandler(IRepository<Vehicle> vehicleRepository)
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
