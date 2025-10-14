using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Queries.GetVehicleSpecs
{
    /// <summary>
    /// Query to get all specifications for a level-2 vehicle model
    /// </summary>
    public record GetVehicleSpecsQuery(string ModelId) : IRequest<List<VehicleSpecDto>>;
}

