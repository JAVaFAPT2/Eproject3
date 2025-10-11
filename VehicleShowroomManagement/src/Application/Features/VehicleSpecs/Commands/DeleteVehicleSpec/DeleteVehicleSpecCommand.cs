using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Commands.DeleteVehicleSpec
{
    /// <summary>
    /// Command to delete a vehicle specification
    /// </summary>
    public record DeleteVehicleSpecCommand(string SpecId) : IRequest;
}

