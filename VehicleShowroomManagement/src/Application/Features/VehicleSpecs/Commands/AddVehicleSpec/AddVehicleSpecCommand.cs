using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Commands.AddVehicleSpec
{
    /// <summary>
    /// Command to add a specification to a vehicle
    /// </summary>
    public record AddVehicleSpecCommand(
        string VehicleId,
        string SpecName,
        string SpecValue,
        int DisplayOrder = 0,
        string? GroupName = null) : IRequest<string>;
}

