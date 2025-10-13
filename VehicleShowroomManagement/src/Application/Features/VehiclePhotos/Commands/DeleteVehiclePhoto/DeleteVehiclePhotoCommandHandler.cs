
namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.DeleteVehiclePhoto
{
    /// <summary>
    /// Handler for deleting a vehicle photo
    /// </summary>
    public class DeleteVehiclePhotoCommandHandler(IRepository<VehiclePhoto> photoRepository) : IRequestHandler<DeleteVehiclePhotoCommand>
    {

        public async Task Handle(DeleteVehiclePhotoCommand request, CancellationToken cancellationToken)
        {
            var photo = await photoRepository.GetByIdAsync(request.PhotoId, cancellationToken) ?? throw new KeyNotFoundException($"Photo with ID {request.PhotoId} not found");
            await photoRepository.DeleteAsync(photo, cancellationToken);
        }
    }
}

