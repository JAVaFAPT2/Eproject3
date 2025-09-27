using MediatR;
using VehicleShowroomManagement.Application.Images.Commands;
using VehicleShowroomManagement.Application.Images.Services;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.Images.Handlers
{
    /// <summary>
    /// Handler for uploading vehicle images
    /// </summary>
    public class UploadVehicleImageCommandHandler : IRequestHandler<UploadVehicleImageCommand, string>
    {
        private readonly IImageUploadService _imageUploadService;
        private readonly IRepository<VehicleImage> _vehicleImageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UploadVehicleImageCommandHandler(
            IImageUploadService imageUploadService,
            IRepository<VehicleImage> vehicleImageRepository,
            IUnitOfWork unitOfWork)
        {
            _imageUploadService = imageUploadService;
            _vehicleImageRepository = vehicleImageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UploadVehicleImageCommand request, CancellationToken cancellationToken)
        {
            using var stream = request.ImageFile.OpenReadStream();
            var fileName = $"{request.VehicleId}_{Guid.NewGuid()}{Path.GetExtension(request.ImageFile.FileName)}";
            
            var publicId = await _imageUploadService.UploadImageAsync(
                stream, 
                fileName, 
                request.Folder, 
                request.Width, 
                request.Height, 
                request.Transformation);

            var vehicleImage = new VehicleImage
            {
                VehicleId = int.Parse(request.VehicleId),
                PublicId = publicId,
                OriginalFileName = request.ImageFile.FileName,
                ContentType = request.ImageFile.ContentType,
                FileSize = (int)request.ImageFile.Length,
                ImageUrl = _imageUploadService.GetOptimizedImageUrl(publicId, request.Width, request.Height, request.Transformation),
                IsPrimary = false,
                CreatedAt = DateTime.UtcNow
            };

            await _vehicleImageRepository.AddAsync(vehicleImage);
            _unitOfWork.SaveChangesAsync();

            return vehicleImage.ImageUrl;
        }
    }
}
