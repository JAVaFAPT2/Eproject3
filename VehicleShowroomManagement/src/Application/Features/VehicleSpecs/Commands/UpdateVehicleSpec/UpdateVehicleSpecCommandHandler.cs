using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Commands.UpdateVehicleSpec
{
    /// <summary>
    /// Handler for updating a vehicle specification
    /// </summary>
    public class UpdateVehicleSpecCommandHandler : IRequestHandler<UpdateVehicleSpecCommand>
    {
        private readonly IRepository<VehicleSpec> _specRepository;

        public UpdateVehicleSpecCommandHandler(IRepository<VehicleSpec> specRepository)
        {
            _specRepository = specRepository;
        }

        public async Task Handle(UpdateVehicleSpecCommand request, CancellationToken cancellationToken)
        {
            var spec = await _specRepository.GetByIdAsync(request.SpecId);
            if (spec == null)
            {
                throw new KeyNotFoundException($"Spec with ID {request.SpecId} not found");
            }

            if (!string.IsNullOrWhiteSpace(request.SpecValue))
            {
                spec.UpdateValue(request.SpecValue);
            }

            if (request.DisplayOrder.HasValue)
            {
                spec.UpdateDisplayOrder(request.DisplayOrder.Value);
            }

            if (request.GroupName != null)
            {
                spec.UpdateGroupName(request.GroupName);
            }

            await _specRepository.UpdateAsync(spec);
        }
    }
}

