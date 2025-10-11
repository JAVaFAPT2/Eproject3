using MediatR;
using MongoDB.Driver;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Queries.GetVehiclePhotos
{
    /// <summary>
    /// Handler for getting all photos for a vehicle
    /// </summary>
    public class GetVehiclePhotosQueryHandler : IRequestHandler<GetVehiclePhotosQuery, List<VehiclePhotoDto>>
    {
        private readonly IRepository<VehiclePhoto> _photoRepository;

        public GetVehiclePhotosQueryHandler(IRepository<VehiclePhoto> photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public async Task<List<VehiclePhotoDto>> Handle(GetVehiclePhotosQuery request, CancellationToken cancellationToken)
        {
            var photos = await _photoRepository.FindAsync(p => p.VehicleId == request.VehicleId, cancellationToken);

            return photos
                .OrderBy(p => p.DisplayOrder)
                .Select(p => new VehiclePhotoDto
                {
                    Id = p.Id,
                    VehicleId = p.VehicleId,
                    Url = p.Url,
                    DisplayOrder = p.DisplayOrder,
                    Caption = p.Caption
                }).ToList();
        }
    }
}

