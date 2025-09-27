using MediatR;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.DeleteVehicle
{
    /// <summary>
    /// Command for deleting a vehicle
    /// </summary>
    public record DeleteVehicleCommand(string Id) : IRequest;
}