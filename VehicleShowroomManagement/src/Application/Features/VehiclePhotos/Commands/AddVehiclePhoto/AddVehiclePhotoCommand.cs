
namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.AddVehiclePhoto
{
    /// <summary>
    /// Command to add a photo to a vehicle model (Level-2)
    /// </summary>
    public record AddVehiclePhotoCommand(
        string ModelNumber,
        string Url,
        int DisplayOrder = 0,
        string? Caption = null) : IRequest<string>;
}

