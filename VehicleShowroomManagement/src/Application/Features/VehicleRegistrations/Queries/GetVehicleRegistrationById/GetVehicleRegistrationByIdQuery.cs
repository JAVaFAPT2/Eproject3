using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleRegistrations.Queries.GetVehicleRegistrationById
{
    public class GetVehicleRegistrationByIdQuery : IRequest<VehicleRegistrationDto>
    {
        public string VehicleRegistrationId { get; set; } = string.Empty;
    }
}