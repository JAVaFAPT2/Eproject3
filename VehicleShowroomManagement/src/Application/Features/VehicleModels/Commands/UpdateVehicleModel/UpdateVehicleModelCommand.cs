using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleModels.Commands.UpdateVehicleModel
{
    /// <summary>
    /// Command to update an existing vehicle model
    /// </summary>
    public record UpdateVehicleModelCommand(
        string ModelNumber,
        string Name,
        string Brand,
        decimal Price,
        string Description,
        string? ImageUrl = null) : IRequest;
}

