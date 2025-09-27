using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.Images.Queries.GetVehicleImages
{
    /// <summary>
    /// Query for getting vehicle images
    /// </summary>
    public record GetVehicleImagesQuery(string VehicleId) : IRequest<List<VehicleImageDto>>;
}