using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Queries.GetVehiclePhotos
{
    /// <summary>
    /// Query to get all photos for a vehicle
    /// </summary>
    public record GetVehiclePhotosQuery(string VehicleId) : IRequest<List<VehiclePhotoDto>>;
}

