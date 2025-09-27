using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Events;

namespace VehicleShowroomManagement.Application.Features.VehicleImages.Commands.CreateVehicleImage
{
    public class CreateVehicleImageCommandHandler : IRequestHandler<CreateVehicleImageCommand, string>
    {
        private readonly IVehicleImageRepository _vehicleImageRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateVehicleImageCommandHandler(
            IVehicleImageRepository vehicleImageRepository,
            IVehicleRepository vehicleRepository,
            IUnitOfWork unitOfWork)
        {
            _vehicleImageRepository = vehicleImageRepository;
            _vehicleRepository = vehicleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateVehicleImageCommand request, CancellationToken cancellationToken)
        {
            // Validate vehicle exists
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
                throw new ArgumentException("Vehicle not found", nameof(request.VehicleId));

            // If this is set as primary, unset any existing primary images for this vehicle
            if (request.IsPrimary)
            {
                var existingImages = await _vehicleImageRepository.GetByVehicleIdAsync(request.VehicleId);
                foreach (var image in existingImages.Where(img => img.IsPrimary))
                {
                    // Create a new instance to avoid modifying the tracked entity
                    var updatedImage = new VehicleImage(
                        image.VehicleId,
                        image.ImageUrl,
                        image.ImageType,
                        image.FileName,
                        image.FileSize,
                        image.PublicId,
                        image.OriginalFileName,
                        image.ContentType,
                        false // Set to false since we're making this one primary
                    );
                    updatedImage.Id = image.Id; // Preserve the ID
                    await _vehicleImageRepository.UpdateAsync(updatedImage);
                }
            }

            // Create vehicle image
            var vehicleImage = new VehicleImage(
                request.VehicleId,
                request.ImageUrl,
                request.ImageType,
                request.FileName,
                request.FileSize,
                request.PublicId,
                request.OriginalFileName,
                request.ContentType,
                request.IsPrimary
            );

            // Add domain events
            vehicleImage.AddDomainEvent(new VehicleImageCreatedEvent(vehicleImage.Id, vehicleImage.VehicleId));

            // Save to repository
            await _vehicleImageRepository.AddAsync(vehicleImage);
            await _unitOfWork.SaveChangesAsync();

            return vehicleImage.Id;
        }
    }
}