using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleSpecs.Commands.UpdateVehicleSpec
{
    /// <summary>
    /// Command to update a vehicle specification
    /// </summary>
    public record UpdateVehicleSpecCommand(
        string SpecId,
        string? SpecValue = null,
        int? DisplayOrder = null,
        string? GroupName = null) : IRequest;
}

