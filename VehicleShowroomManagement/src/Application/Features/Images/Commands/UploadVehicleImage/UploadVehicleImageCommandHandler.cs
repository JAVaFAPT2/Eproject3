using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Images.Commands.UploadVehicleImage
{
    /// <summary>
    /// Handler for upload vehicle image command
    /// </summary>
    public class UploadVehicleImageCommandHandler : IRequestHandler<UploadVehicleImageCommand, List<VehicleImageResult>>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRepository<VehicleImage> _vehicleImageRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public UploadVehicleImageCommandHandler(
            IVehicleRepository vehicleRepository,
            IRepository<VehicleImage> vehicleImageRepository,
            ICloudinaryService cloudinaryService)
        {
            _vehicleRepository = vehicleRepository;
            _vehicleImageRepository = vehicleImageRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<List<VehicleImageResult>> Handle(UploadVehicleImageCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
            {
                throw new ArgumentException("Vehicle not found");
            }

            var results = new List<VehicleImageResult>();

            foreach (var image in request.Images)
            {
                // Upload to Cloudinary
                var uploadResult = await _cloudinaryService.UploadImageAsync(image, $"vehicles/{request.VehicleId}");

                // Create vehicle image entity
                var vehicleImage = new VehicleImage
                {
                    Id = Guid.NewGuid().ToString(),
                    VehicleId = request.VehicleId,
                    ImageUrl = uploadResult.SecureUrl,
                    PublicId = uploadResult.PublicId,
                    FileName = image.FileName,
                    FileSize = (int)image.Length,
                    ContentType = image.ContentType,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _vehicleImageRepository.AddAsync(vehicleImage);

                results.Add(new VehicleImageResult
                {
                    ImageId = vehicleImage.Id,
                    ImageUrl = vehicleImage.ImageUrl,
                    FileName = vehicleImage.FileName,
                    FileSize = vehicleImage.FileSize
                });
            }

            return results;
        }
    }
}