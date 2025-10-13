namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.UpdateVehiclePhoto
{
    /// <summary>
    /// Handler for updating a vehicle photo
    /// </summary>
    public class UpdateVehiclePhotoCommandHandler(IRepository<VehiclePhoto> photoRepository) : IRequestHandler<UpdateVehiclePhotoCommand>
    {
        private readonly IRepository<VehiclePhoto> _photoRepository = photoRepository;

        public async Task Handle(UpdateVehiclePhotoCommand request, CancellationToken cancellationToken)
        {
            var photo = await _photoRepository.GetByIdAsync(request.PhotoId, cancellationToken);
            if (photo == null)
            {
                throw new KeyNotFoundException($"Photo with ID {request.PhotoId} not found");
            }

            if (!string.IsNullOrWhiteSpace(request.Url))
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

            await _photoRepository.UpdateAsync(photo, cancellationToken);
        }
    }
}

