using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.DeleteVehicle
{
    /// <summary>
    /// Handler for delete vehicle command
    /// </summary>
    public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand>
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public DeleteVehicleCommandHandler(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.Id);
            if (vehicle == null)
            {
                throw new ArgumentException("Vehicle not found");
            }

            await _vehicleRepository.DeleteAsync(request.Id);
        }
    }
}
