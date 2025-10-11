using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Queries.GetSpecById
{
    /// <summary>
    /// Query to get a specification by ID
    /// </summary>
    public record GetSpecByIdQuery(string SpecId) : IRequest<VehicleSpecDto?>;
}

