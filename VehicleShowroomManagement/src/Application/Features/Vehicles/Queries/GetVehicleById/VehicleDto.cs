using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Queries.GetVehicleById
{
    /// <summary>
    /// Vehicle data transfer object
    /// </summary>
    public class VehicleDto
    {
        public string Id { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string? ExternalNumber { get; set; }
        public string? Vin { get; set; }
        public string? LicensePlate { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public VehicleStatus Status { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal? SalePrice { get; set; }
        public List<string> Photos { get; set; } = new List<string>();
        public DateTime ReceiptDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Computed properties
        public bool IsAvailable { get; set; }
        public bool IsSold { get; set; }
        public bool IsReserved { get; set; }
        public bool IsRegistered { get; set; }
    }
}