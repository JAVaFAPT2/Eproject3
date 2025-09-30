using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Queries.GetVehicleById
{
    /// <summary>
    /// Query to get a vehicle by ID
    /// </summary>
    public record GetVehicleByIdQuery(string VehicleId) : IRequest<VehicleDto?>;
}