using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleModels.Commands.CreateVehicleModel
{
    public record CreateVehicleModelCommand(
        string ModelNumber,
        string Name,
        string Brand,
        decimal Price) : IRequest<string>;
}

