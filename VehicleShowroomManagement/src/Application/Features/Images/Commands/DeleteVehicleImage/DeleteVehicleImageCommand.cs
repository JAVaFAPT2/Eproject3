using MediatR;

namespace VehicleShowroomManagement.Application.Features.Images.Commands.DeleteVehicleImage
{
    /// <summary>
    /// Command for deleting vehicle image
    /// </summary>
    public record DeleteVehicleImageCommand(string VehicleId, string ImageId) : IRequest;
}