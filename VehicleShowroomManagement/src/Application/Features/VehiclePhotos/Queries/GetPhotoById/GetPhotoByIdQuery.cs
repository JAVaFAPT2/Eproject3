using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Queries.GetPhotoById
{
    /// <summary>
    /// Query to get a photo by ID
    /// </summary>
    public record GetPhotoByIdQuery(string PhotoId) : IRequest<VehiclePhotoDto?>;
}

