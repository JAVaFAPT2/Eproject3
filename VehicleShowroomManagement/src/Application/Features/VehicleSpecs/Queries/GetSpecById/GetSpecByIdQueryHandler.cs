using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Queries.GetSpecById
{
    /// <summary>
    /// Handler for getting a specification by ID
    /// </summary>
    public class GetSpecByIdQueryHandler(IRepository<VehicleSpec> specRepository) : IRequestHandler<GetSpecByIdQuery, VehicleSpecDto?>
    {

        public async Task<VehicleSpecDto?> Handle(GetSpecByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = await specRepository.GetByIdAsync(request.SpecId, cancellationToken);
            
            if (spec is null)
                return null;

            return new VehicleSpecDto
            {
                Id = spec.Id,
                ModelId = spec.ModelId,
                SpecName = spec.SpecName,
                SpecValue = spec.SpecValue,
                DisplayOrder = spec.DisplayOrder,
                GroupName = spec.GroupName
            };
        }
    }
}

