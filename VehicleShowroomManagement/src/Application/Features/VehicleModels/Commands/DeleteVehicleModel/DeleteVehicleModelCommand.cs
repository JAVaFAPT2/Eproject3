using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleModels.Commands.DeleteVehicleModel
{
    /// <summary>
    /// Command to soft delete a vehicle model by model number
    /// </summary>
    public record DeleteVehicleModelCommand(string ModelNumber) : IRequest<Unit>;
}


