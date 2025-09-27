using MediatR;

namespace VehicleShowroomManagement.Application.VehicleRegistrations.Commands
{
    /// <summary>
    /// Command to update an existing vehicle registration
    /// </summary>
    public class UpdateVehicleRegistrationCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
        public string RegistrationAuthority { get; set; } = string.Empty;
        public string RegistrationState { get; set; } = string.Empty;
        public string RegistrationCity { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string OwnerAddress { get; set; } = string.Empty;
        public string? OwnerPhone { get; set; }
        public string? OwnerEmail { get; set; }
        public string? InsuranceNumber { get; set; }
        public DateTime? InsuranceExpiry { get; set; }
        public string? PollutionCertificateNumber { get; set; }
        public DateTime? PollutionCertificateExpiry { get; set; }
        public string? FitnessCertificateNumber { get; set; }
        public DateTime? FitnessCertificateExpiry { get; set; }
        public string? Notes { get; set; }
    }
}
