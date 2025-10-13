using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.AddVehiclePhoto
{
    /// <summary>
    /// Command to add a photo to a vehicle or vehicle model
    /// </summary>
    public record AddVehiclePhotoCommand(
        string? VehicleId,
        string? VehicleModelId,
        string Url,
        int DisplayOrder = 0,
        string? Caption = null) : IRequest<string>;
}

