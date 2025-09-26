using MediatR;

namespace VehicleShowroomManagement.Application.VehicleRegistrations.Queries
{
    /// <summary>
    /// Query to get a vehicle registration by ID
    /// </summary>
    public class GetVehicleRegistrationByIdQuery : IRequest<VehicleRegistrationDto?>
    {
        public string Id { get; set; } = string.Empty;
    }
}
