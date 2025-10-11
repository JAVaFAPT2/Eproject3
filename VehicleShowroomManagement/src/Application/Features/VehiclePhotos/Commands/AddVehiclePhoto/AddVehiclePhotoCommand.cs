using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.AddVehiclePhoto
{
    /// <summary>
    /// Command to add a photo to a vehicle
    /// </summary>
    public record AddVehiclePhotoCommand(
        string VehicleId,
        string Url,
        int DisplayOrder = 0,
        string? Caption = null) : IRequest<string>;
}

