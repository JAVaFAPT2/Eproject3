using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.UpdateVehicleStatus
{
    /// <summary>
    /// Command to update vehicle status - critical for inventory management
    /// </summary>
    public record UpdateVehicleStatusCommand : IRequest
    {
        public string VehicleId { get; init; }
        public VehicleStatus Status { get; init; }

        public UpdateVehicleStatusCommand(string vehicleId, VehicleStatus status)
        {
            VehicleId = vehicleId;
            Status = status;
        }
    }
}