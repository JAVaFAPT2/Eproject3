using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModelById
{
    /// <summary>
    /// Query to get a vehicle model by model number
    /// </summary>
    public record GetVehicleModelByIdQuery(string ModelNumber) : IRequest<VehicleModelDto?>;
}
