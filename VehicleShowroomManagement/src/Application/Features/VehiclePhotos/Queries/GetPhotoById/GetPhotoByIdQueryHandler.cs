using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Queries.GetPhotoById
{
    /// <summary>
    /// Handler for getting a photo by ID
    /// </summary>
    public class GetPhotoByIdQueryHandler(IRepository<VehiclePhoto> photoRepository) : IRequestHandler<GetPhotoByIdQuery, VehiclePhotoDto?>
    {

        public async Task<VehiclePhotoDto?> Handle(GetPhotoByIdQuery request, CancellationToken cancellationToken)
        {
            var photo = await photoRepository.GetByIdAsync(request.PhotoId, cancellationToken);
            
            if (photo is null)
                return null;

            return new VehiclePhotoDto
            {
                Id = photo.Id,
                VehicleId = string.Empty,
                VehicleModelId = photo.ModelNumber,
                Url = photo.Url,
                DisplayOrder = photo.DisplayOrder,
                Caption = photo.Caption
            };
        }
    }
}

