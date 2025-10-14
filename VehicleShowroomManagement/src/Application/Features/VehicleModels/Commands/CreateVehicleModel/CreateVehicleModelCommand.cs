using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleModels.Commands.CreateVehicleModel
{
    public record CreateVehicleModelCommand(
        string ModelNumber,
        string Name,
        decimal Price,
        string Description,
        string? ParentId = null,
        int Level = 1,
        string? Slug = null) : IRequest<string>;
}

