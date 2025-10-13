namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.AddVehiclePhoto
{
    /// <summary>
    /// Handler for adding a photo to a vehicle
    /// </summary>
    public class AddVehiclePhotoCommandHandler(
        IRepository<VehiclePhoto> photoRepository,
        IRepository<Vehicle> vehicleRepository) : IRequestHandler<AddVehiclePhotoCommand, string>
    {

        public async Task<string> Handle(AddVehiclePhotoCommand request, CancellationToken cancellationToken)
        {
            // Verify vehicle exists
            _ = await vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken) ?? throw new KeyNotFoundException($"Vehicle with ID {request.VehicleId} not found");

            // Create photo
            var photo = new VehiclePhoto(
                request.VehicleId,
                request.Url,
                request.DisplayOrder,
                request.Caption);

            await photoRepository.AddAsync(photo, cancellationToken);

            return photo.Id;
        }
    }
}

