using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Images.Commands.DeleteVehicleImage
{
    /// <summary>
    /// Handler for delete vehicle image command
    /// </summary>
    public class DeleteVehicleImageCommandHandler : IRequestHandler<DeleteVehicleImageCommand>
    {
        private readonly IRepository<VehicleImage> _vehicleImageRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public DeleteVehicleImageCommandHandler(
            IRepository<VehicleImage> vehicleImageRepository,
            ICloudinaryService cloudinaryService)
        {
            _vehicleImageRepository = vehicleImageRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task Handle(DeleteVehicleImageCommand request, CancellationToken cancellationToken)
        {
            var vehicleImages = await _vehicleImageRepository.GetAllAsync();
            var vehicleImage = vehicleImages.FirstOrDefault(vi => 
                vi.VehicleId == request.VehicleId && vi.Id == request.ImageId);

            if (vehicleImage == null)
            {
                throw new ArgumentException("Vehicle image not found");
            }

            // Delete from Cloudinary
            await _cloudinaryService.DeleteImageAsync(vehicleImage.PublicId);

            // Delete from database
            await _vehicleImageRepository.DeleteAsync(vehicleImage.Id);
        }
    }
}