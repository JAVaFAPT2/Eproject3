using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Commands.AddVehicleSpec
{
    /// <summary>
    /// Handler for adding a specification to a vehicle
    /// </summary>
    public class AddVehicleSpecCommandHandler : IRequestHandler<AddVehicleSpecCommand, string>
    {
        private readonly IRepository<VehicleSpec> _specRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public AddVehicleSpecCommandHandler(
            IRepository<VehicleSpec> specRepository,
            IRepository<Vehicle> vehicleRepository)
        {
            _specRepository = specRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<string> Handle(AddVehicleSpecCommand request, CancellationToken cancellationToken)
        {
            // Verify vehicle exists
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {request.VehicleId} not found");
            }

            // Create spec
            var spec = new VehicleSpec(
                request.VehicleId,
                request.SpecName,
                request.SpecValue,
                request.DisplayOrder,
                request.GroupName);

            await _specRepository.AddAsync(spec);

            return spec.Id;
        }
    }
}

