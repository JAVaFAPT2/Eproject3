namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.AddVehiclePhoto
{
    /// <summary>
    /// Handler for adding a photo to a vehicle
    /// </summary>
    public class AddVehiclePhotoCommandHandler(
        IRepository<VehiclePhoto> photoRepository,
        IRepository<VehicleModel> vehicleModelRepository) : IRequestHandler<AddVehiclePhotoCommand, string>
    {

        public async Task<string> Handle(AddVehiclePhotoCommand request, CancellationToken cancellationToken)
        {
            // Verify Level-2 model exists by model number
            _ = await vehicleModelRepository.GetByIdAsync(request.ModelNumber, cancellationToken)
                ?? throw new KeyNotFoundException($"VehicleModel with number {request.ModelNumber} not found");

            // Create photo
            var photo = new VehiclePhoto(
                request.ModelNumber,
                request.Url,
                request.DisplayOrder,
                request.Caption);

            await photoRepository.AddAsync(photo, cancellationToken);

            return photo.Id;
        }
    }
}

