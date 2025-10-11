using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Queries.GetSpecById
{
    /// <summary>
    /// Handler for getting a specification by ID
    /// </summary>
    public class GetSpecByIdQueryHandler : IRequestHandler<GetSpecByIdQuery, VehicleSpecDto?>
    {
        private readonly IRepository<VehicleSpec> _specRepository;

        public GetSpecByIdQueryHandler(IRepository<VehicleSpec> specRepository)
        {
            _specRepository = specRepository;
        }

        public async Task<VehicleSpecDto?> Handle(GetSpecByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = await _specRepository.GetByIdAsync(request.SpecId);
            
            if (spec == null)
                return null;

            return new VehicleSpecDto
            {
                Id = spec.Id,
                VehicleId = spec.VehicleId,
                SpecName = spec.SpecName,
                SpecValue = spec.SpecValue,
                DisplayOrder = spec.DisplayOrder,
                GroupName = spec.GroupName
            };
        }
    }
}

