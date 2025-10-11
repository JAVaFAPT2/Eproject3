using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.DeleteVehiclePhoto
{
    /// <summary>
    /// Command to delete a vehicle photo
    /// </summary>
    public record DeleteVehiclePhotoCommand(string PhotoId) : IRequest;
}

