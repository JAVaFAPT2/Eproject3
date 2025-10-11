using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Queries.GetVehicleSpecs
{
    /// <summary>
    /// Query to get all specifications for a vehicle
    /// </summary>
    public record GetVehicleSpecsQuery(string VehicleId) : IRequest<List<VehicleSpecDto>>;
}

