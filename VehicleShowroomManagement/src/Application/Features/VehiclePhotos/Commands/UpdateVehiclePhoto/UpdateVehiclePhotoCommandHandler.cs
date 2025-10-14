namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.UpdateVehiclePhoto
{
    /// <summary>
    /// Handler for updating a vehicle photo
    /// </summary>
    public class UpdateVehiclePhotoCommandHandler(IRepository<VehiclePhoto> photoRepository) : IRequestHandler<UpdateVehiclePhotoCommand>
    {

        public async Task Handle(UpdateVehiclePhotoCommand request, CancellationToken cancellationToken)
        {
            var photo = await photoRepository.GetByIdAsync(request.PhotoId, cancellationToken);
            if (photo is null)
            {
                throw new KeyNotFoundException($"Photo with ID {request.PhotoId} not found");
            }


            if (request.Url != null)
            {
                photo.UpdateUrl(request.Url);
            }

            if (request.DisplayOrder.HasValue)
            {
                photo.UpdateDisplayOrder(request.DisplayOrder.Value);
            }

            if (request.Caption != null)
            {
                photo.UpdateCaption(request.Caption);
            }

            await photoRepository.UpdateAsync(photo, cancellationToken);
        }
    }
}

