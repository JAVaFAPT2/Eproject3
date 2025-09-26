namespace VehicleShowroomManagement.Application.Reports.DTOs
{
    /// <summary>
    /// Data Transfer Object for Stock Availability Report
    /// </summary>
    public class StockAvailabilityReportDto
    {
        public DateTime GeneratedAt { get; set; }
        public DateTime AsOfDate { get; set; }
        public int TotalVehicles { get; set; }
        public int AvailableVehicles { get; set; }
        public int SoldVehicles { get; set; }
        public int ReservedVehicles { get; set; }
        public int InServiceVehicles { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AvailableValue { get; set; }
        public List<BrandStockDto> BrandStocks { get; set; } = new List<BrandStockDto>();
        public List<ModelStockDto> ModelStocks { get; set; } = new List<ModelStockDto>();
        public List<StatusStockDto> StatusStocks { get; set; } = new List<StatusStockDto>();
    }

    public class BrandStockDto
    {
        public string Brand { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public int AvailableCount { get; set; }
        public int SoldCount { get; set; }
        public int ReservedCount { get; set; }
        public int InServiceCount { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AvailableValue { get; set; }
    }

    public class ModelStockDto
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public int AvailableCount { get; set; }
        public int SoldCount { get; set; }
        public int ReservedCount { get; set; }
        public int InServiceCount { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AvailableValue { get; set; }
        public decimal AveragePrice { get; set; }
    }

    public class StatusStockDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
    }
}
