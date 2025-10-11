using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Queries.GetPhotoById
{
    /// <summary>
    /// Handler for getting a photo by ID
    /// </summary>
    public class GetPhotoByIdQueryHandler : IRequestHandler<GetPhotoByIdQuery, VehiclePhotoDto?>
    {
        private readonly IRepository<VehiclePhoto> _photoRepository;

        public GetPhotoByIdQueryHandler(IRepository<VehiclePhoto> photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public async Task<VehiclePhotoDto?> Handle(GetPhotoByIdQuery request, CancellationToken cancellationToken)
        {
            var photo = await _photoRepository.GetByIdAsync(request.PhotoId);
            
            if (photo == null)
                return null;

            return new VehiclePhotoDto
            {
                Id = photo.Id,
                VehicleId = photo.VehicleId,
                Url = photo.Url,
                DisplayOrder = photo.DisplayOrder,
                Caption = photo.Caption
            };
        }
    }
}

