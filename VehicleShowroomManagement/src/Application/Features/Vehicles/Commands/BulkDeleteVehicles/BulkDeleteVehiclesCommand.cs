using MediatR;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.BulkDeleteVehicles
{
    /// <summary>
    /// Command for bulk deleting vehicles
    /// </summary>
    public record BulkDeleteVehiclesCommand(List<string> VehicleIds) : IRequest;
}