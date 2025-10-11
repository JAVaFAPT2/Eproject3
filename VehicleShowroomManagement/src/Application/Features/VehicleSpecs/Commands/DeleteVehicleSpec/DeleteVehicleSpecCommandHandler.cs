using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Commands.DeleteVehicleSpec
{
    /// <summary>
    /// Handler for deleting a vehicle specification
    /// </summary>
    public class DeleteVehicleSpecCommandHandler : IRequestHandler<DeleteVehicleSpecCommand>
    {
        private readonly IRepository<VehicleSpec> _specRepository;

        public DeleteVehicleSpecCommandHandler(IRepository<VehicleSpec> specRepository)
        {
            _specRepository = specRepository;
        }

        public async Task Handle(DeleteVehicleSpecCommand request, CancellationToken cancellationToken)
        {
            var spec = await _specRepository.GetByIdAsync(request.SpecId);
            if (spec == null)
            {
                throw new KeyNotFoundException($"Spec with ID {request.SpecId} not found");
            }

            await _specRepository.DeleteAsync(request.SpecId);
        }
    }
}

