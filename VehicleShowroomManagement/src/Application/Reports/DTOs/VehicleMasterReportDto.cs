namespace VehicleShowroomManagement.Application.Reports.DTOs
{
    /// <summary>
    /// Data Transfer Object for Vehicle Master Information Report
    /// </summary>
    public class VehicleMasterReportDto
    {
        public DateTime GeneratedAt { get; set; }
        public int TotalVehicles { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
        public List<VehicleDetailDto> Vehicles { get; set; } = new List<VehicleDetailDto>();
        public List<BrandSummaryDto> BrandSummaries { get; set; } = new List<BrandSummaryDto>();
        public List<ModelSummaryDto> ModelSummaries { get; set; } = new List<ModelSummaryDto>();
        public List<StatusSummaryDto> StatusSummaries { get; set; } = new List<StatusSummaryDto>();
        public List<YearSummaryDto> YearSummaries { get; set; } = new List<YearSummaryDto>();
    }

    public class VehicleDetailDto
    {
        public string Id { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Color { get; set; } = string.Empty;
        public int Mileage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public VehicleRegistrationDto? Registration { get; set; }
        public List<ServiceOrderDto> ServiceHistory { get; set; } = new List<ServiceOrderDto>();
    }

    public class VehicleRegistrationDto
    {
        public string RegistrationNumber { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string RegistrationState { get; set; } = string.Empty;
        public string RegistrationCity { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class ServiceOrderDto
    {
        public string Id { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalCost { get; set; }
        public string? Description { get; set; }
    }

    public class BrandSummaryDto
    {
        public string Brand { get; set; } = string.Empty;
        public int VehicleCount { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
        public int AvailableCount { get; set; }
        public int SoldCount { get; set; }
        public int ReservedCount { get; set; }
        public int InServiceCount { get; set; }
    }

    public class ModelSummaryDto
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int VehicleCount { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int AvailableCount { get; set; }
        public int SoldCount { get; set; }
    }

    public class StatusSummaryDto
    {
        public string Status { get; set; } = string.Empty;
        public int VehicleCount { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal Percentage { get; set; }
    }

    public class YearSummaryDto
    {
        public int Year { get; set; }
        public int VehicleCount { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
        public int AvailableCount { get; set; }
        public int SoldCount { get; set; }
    }
}
