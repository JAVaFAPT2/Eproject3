using MediatR;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Queries.GetVehicleById
{
    /// <summary>
    /// Query to get a vehicle by ID
    /// </summary>
    public record GetVehicleByIdQuery : IRequest<VehicleDto?>
    {
        public string VehicleId { get; init; }

        public GetVehicleByIdQuery(string vehicleId)
        {
            VehicleId = vehicleId;
        }
    }
}