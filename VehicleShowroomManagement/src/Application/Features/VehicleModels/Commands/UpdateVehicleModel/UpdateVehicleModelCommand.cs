using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleModels.Commands.UpdateVehicleModel
{
    /// <summary>
    /// Command to update an existing vehicle model
    /// </summary>
    public record UpdateVehicleModelCommand(
        string ModelNumber,
        string Name,
        decimal Price,
        string Description,
        string? ParentId = null,
        int Level = 1,
        string? Slug = null) : IRequest;
}

