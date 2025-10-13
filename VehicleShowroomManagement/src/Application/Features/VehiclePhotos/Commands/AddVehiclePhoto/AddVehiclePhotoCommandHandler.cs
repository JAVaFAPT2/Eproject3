namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.AddVehiclePhoto
{
    /// <summary>
    /// Handler for adding a photo to a vehicle
    /// </summary>
    public class AddVehiclePhotoCommandHandler(
        IRepository<VehiclePhoto> photoRepository,
        IRepository<Vehicle> vehicleRepository,
        IRepository<VehicleModel> vehicleModelRepository) : IRequestHandler<AddVehiclePhotoCommand, string>
    {

        public async Task<string> Handle(AddVehiclePhotoCommand request, CancellationToken cancellationToken)
        {
            // Verify target exists (vehicle or model)
            if (!string.IsNullOrWhiteSpace(request.VehicleId))
            {
                _ = await vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken) ?? throw new KeyNotFoundException($"Vehicle with ID {request.VehicleId} not found");
            }
            else if (!string.IsNullOrWhiteSpace(request.VehicleModelId))
            {
                _ = await vehicleModelRepository.GetByIdAsync(request.VehicleModelId, cancellationToken) ?? throw new KeyNotFoundException($"VehicleModel with ID {request.VehicleModelId} not found");
            }
            else
            {
                throw new ArgumentException("Either VehicleId or VehicleModelId must be provided");
            }

            // Create photo
            var photo = new VehiclePhoto(
                request.VehicleId ?? string.Empty,
                request.VehicleModelId,
                request.Url,
                request.DisplayOrder,
                request.Caption);

            await photoRepository.AddAsync(photo, cancellationToken);

            return photo.Id;
        }
    }
}

