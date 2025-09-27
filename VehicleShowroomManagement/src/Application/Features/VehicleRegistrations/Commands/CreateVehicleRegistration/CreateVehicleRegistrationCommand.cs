using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleRegistrations.Commands.CreateVehicleRegistration
{
    public class CreateVehicleRegistrationCommand : IRequest<string>
    {
        public string VehicleId { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string RegistrationAuthority { get; set; } = string.Empty;
        public string RegistrationState { get; set; } = string.Empty;
        public string RegistrationCity { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string OwnerAddress { get; set; } = string.Empty;
        public string? OwnerPhone { get; set; }
        public string? OwnerEmail { get; set; }
        public string VehicleType { get; set; } = "Car";
        public string FuelType { get; set; } = "Petrol";
        public string? EngineNumber { get; set; }
        public string? ChassisNumber { get; set; }
        public int ManufacturingYear { get; set; }
        public int? ManufacturingMonth { get; set; }
        public int? SeatingCapacity { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? UnladenWeight { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}