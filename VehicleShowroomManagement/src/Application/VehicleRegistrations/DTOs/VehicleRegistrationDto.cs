namespace VehicleShowroomManagement.Application.VehicleRegistrations.DTOs
{
    /// <summary>
    /// Data Transfer Object for Vehicle Registration
    /// </summary>
    public class VehicleRegistrationDto
    {
        public string Id { get; set; } = string.Empty;
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
        public string VehicleType { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public string? EngineNumber { get; set; }
        public string? ChassisNumber { get; set; }
        public int ManufacturingYear { get; set; }
        public int? ManufacturingMonth { get; set; }
        public int? SeatingCapacity { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? UnladenWeight { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsTransferable { get; set; }
        public string? PreviousOwner { get; set; }
        public DateTime? TransferDate { get; set; }
        public string? InsuranceNumber { get; set; }
        public DateTime? InsuranceExpiry { get; set; }
        public string? PollutionCertificateNumber { get; set; }
        public DateTime? PollutionCertificateExpiry { get; set; }
        public string? FitnessCertificateNumber { get; set; }
        public DateTime? FitnessCertificateExpiry { get; set; }
        public string? Notes { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
