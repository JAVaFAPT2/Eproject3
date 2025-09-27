using MediatR;
using VehicleShowroomManagement.Application.VehicleRegistrations.DTOs;

namespace VehicleShowroomManagement.Application.VehicleRegistrations.Queries
{
    /// <summary>
    /// Query to get a vehicle registration by VIN
    /// </summary>
    public class GetVehicleRegistrationByVinQuery : IRequest<VehicleRegistrationDto?>
    {
        public string VIN { get; set; } = string.Empty;
    }
}
