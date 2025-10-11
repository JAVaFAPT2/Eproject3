using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.UpdateVehiclePhoto
{
    /// <summary>
    /// Command to update a vehicle photo
    /// </summary>
    public record UpdateVehiclePhotoCommand(
        string PhotoId,
        string? Url = null,
        int? DisplayOrder = null,
        string? Caption = null) : IRequest;
}

