using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Queries.GetVehicleSpecs
{
    /// <summary>
    /// Handler for getting all specifications for a vehicle
    /// </summary>
    public class GetVehicleSpecsQueryHandler(IRepository<VehicleSpec> specRepository) : IRequestHandler<GetVehicleSpecsQuery, List<VehicleSpecDto>>
    {

        public async Task<List<VehicleSpecDto>> Handle(GetVehicleSpecsQuery request, CancellationToken cancellationToken)
        {
            var specs = await specRepository.FindAsync(s => s.ModelId == request.ModelId, cancellationToken);

            return [.. specs
                .OrderBy(s => s.DisplayOrder)
                .Select(s => new VehicleSpecDto
                {
                    Id = s.Id,
                    ModelId = s.ModelId,
                    SpecName = s.SpecName,
                    SpecValue = s.SpecValue,
                    DisplayOrder = s.DisplayOrder,
                    GroupName = s.GroupName
                })];
        }
    }
}

