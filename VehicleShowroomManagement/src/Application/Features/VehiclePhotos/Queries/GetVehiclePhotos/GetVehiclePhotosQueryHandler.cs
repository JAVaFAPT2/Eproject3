using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Queries.GetVehiclePhotos
{
    /// <summary>
    /// Handler for getting all photos for a vehicle
    /// </summary>
    public class GetVehiclePhotosQueryHandler(IRepository<VehiclePhoto> photoRepository) : IRequestHandler<GetVehiclePhotosQuery, List<VehiclePhotoDto>>
    {

        public async Task<List<VehiclePhotoDto>> Handle(GetVehiclePhotosQuery request, CancellationToken cancellationToken)
        {
            var photos = await photoRepository.FindAsync(p => p.VehicleId == request.VehicleId, cancellationToken);

            return [.. photos
                .OrderBy(p => p.DisplayOrder)
                .Select(p => new VehiclePhotoDto
                {
                    Id = p.Id,
                    VehicleId = p.VehicleId,
                    VehicleModelId = p.VehicleModelId,
                    Url = p.Url,
                    DisplayOrder = p.DisplayOrder,
                    Caption = p.Caption
                })];
        }
    }
}

