using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.UpdateVehicleStatus
{
    /// <summary>
    /// Command to update vehicle status
    /// </summary>
    public record UpdateVehicleStatusCommand(
        string VehicleId,
        VehicleStatus Status) : IRequest<bool>;
}