using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.AddVehiclePhoto
{
    /// <summary>
    /// Handler for adding a photo to a vehicle
    /// </summary>
    public class AddVehiclePhotoCommandHandler : IRequestHandler<AddVehiclePhotoCommand, string>
    {
        private readonly IRepository<VehiclePhoto> _photoRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public AddVehiclePhotoCommandHandler(
            IRepository<VehiclePhoto> photoRepository,
            IRepository<Vehicle> vehicleRepository)
        {
            _photoRepository = photoRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<string> Handle(AddVehiclePhotoCommand request, CancellationToken cancellationToken)
        {
            // Verify vehicle exists
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {request.VehicleId} not found");
            }

            // Create photo
            var photo = new VehiclePhoto(
                request.VehicleId,
                request.Url,
                request.DisplayOrder,
                request.Caption);

            await _photoRepository.AddAsync(photo);

            return photo.Id;
        }
    }
}

