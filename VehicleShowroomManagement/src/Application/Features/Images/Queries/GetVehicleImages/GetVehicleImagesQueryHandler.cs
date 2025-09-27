using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Images.Queries.GetVehicleImages
{
    /// <summary>
    /// Handler for get vehicle images query
    /// </summary>
    public class GetVehicleImagesQueryHandler : IRequestHandler<GetVehicleImagesQuery, List<VehicleImageDto>>
    {
        private readonly IRepository<VehicleImage> _vehicleImageRepository;

        public GetVehicleImagesQueryHandler(IRepository<VehicleImage> vehicleImageRepository)
        {
            _vehicleImageRepository = vehicleImageRepository;
        }

        public async Task<List<VehicleImageDto>> Handle(GetVehicleImagesQuery request, CancellationToken cancellationToken)
        {
            var vehicleImages = await _vehicleImageRepository.GetAllAsync();
            var images = vehicleImages.Where(vi => vi.VehicleId == request.VehicleId).ToList();

            return images.Select(vi => new VehicleImageDto
            {
                ImageId = vi.Id,
                ImageUrl = vi.ImageUrl,
                ImageType = vi.ContentType,
                FileName = vi.FileName,
                FileSize = vi.FileSize,
                UploadedAt = vi.CreatedAt
            }).ToList();
        }
    }
}