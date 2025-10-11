using MediatR;
using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Queries.GetVehicleSpecs
{
    /// <summary>
    /// Handler for getting all specifications for a vehicle
    /// </summary>
    public class GetVehicleSpecsQueryHandler : IRequestHandler<GetVehicleSpecsQuery, List<VehicleSpecDto>>
    {
        private readonly IRepository<VehicleSpec> _specRepository;

        public GetVehicleSpecsQueryHandler(IRepository<VehicleSpec> specRepository)
        {
            _specRepository = specRepository;
        }

        public async Task<List<VehicleSpecDto>> Handle(GetVehicleSpecsQuery request, CancellationToken cancellationToken)
        {
            var specs = await _specRepository.FindAsync(s => s.VehicleId == request.VehicleId, cancellationToken);

            return specs
                .OrderBy(s => s.DisplayOrder)
                .Select(s => new VehicleSpecDto
                {
                    Id = s.Id,
                    VehicleId = s.VehicleId,
                    SpecName = s.SpecName,
                    SpecValue = s.SpecValue,
                    DisplayOrder = s.DisplayOrder,
                    GroupName = s.GroupName
                }).ToList();
        }
    }
}

