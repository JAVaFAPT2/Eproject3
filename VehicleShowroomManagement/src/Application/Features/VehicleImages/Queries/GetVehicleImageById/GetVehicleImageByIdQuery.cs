using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleImages.Queries.GetVehicleImageById
{
    public class GetVehicleImageByIdQuery : IRequest<VehicleImageDto>
    {
        public string VehicleImageId { get; set; } = string.Empty;
    }
}