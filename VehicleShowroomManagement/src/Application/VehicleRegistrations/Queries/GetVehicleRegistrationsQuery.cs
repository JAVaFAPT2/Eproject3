using MediatR;

namespace VehicleShowroomManagement.Application.VehicleRegistrations.Queries
{
    /// <summary>
    /// Query to get all vehicle registrations with filtering and pagination
    /// </summary>
    public class GetVehicleRegistrationsQuery : IRequest<IEnumerable<VehicleRegistrationDto>>
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? RegistrationState { get; set; }
        public string? VehicleType { get; set; }
        public string? FuelType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
