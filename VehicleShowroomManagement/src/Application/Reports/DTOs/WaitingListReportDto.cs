namespace VehicleShowroomManagement.Application.Reports.DTOs
{
    /// <summary>
    /// Data Transfer Object for Waiting List Details Report
    /// </summary>
    public class WaitingListReportDto
    {
        public DateTime GeneratedAt { get; set; }
        public int TotalWaitingListEntries { get; set; }
        public int ActiveEntries { get; set; }
        public int NotifiedEntries { get; set; }
        public int ConvertedEntries { get; set; }
        public int CancelledEntries { get; set; }
        public int ExpiredEntries { get; set; }
        public int HighPriorityEntries { get; set; }
        public int MediumPriorityEntries { get; set; }
        public int LowPriorityEntries { get; set; }
        public List<WaitingListDetailDto> WaitingListEntries { get; set; } = new List<WaitingListDetailDto>();
        public List<BrandWaitingDto> BrandWaiting { get; set; } = new List<BrandWaitingDto>();
        public List<ModelWaitingDto> ModelWaiting { get; set; } = new List<ModelWaitingDto>();
        public List<PriorityWaitingDto> PriorityWaiting { get; set; } = new List<PriorityWaitingDto>();
        public List<MonthlyWaitingDto> MonthlyTrends { get; set; } = new List<MonthlyWaitingDto>();
    }

    public class WaitingListDetailDto
    {
        public string Id { get; set; } = string.Empty;
        public string WaitingListNumber { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string? ModelId { get; set; }
        public string? Brand { get; set; }
        public string? ModelName { get; set; }
        public string? VehicleType { get; set; }
        public string? PreferredColor { get; set; }
        public int? PreferredYear { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public int Priority { get; set; }
        public int Position { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime? ExpectedAvailabilityDate { get; set; }
        public DateTime? LastContactDate { get; set; }
        public DateTime? NextContactDate { get; set; }
        public string ContactMethod { get; set; } = string.Empty;
        public string ContactFrequency { get; set; } = string.Empty;
        public bool IsFlexible { get; set; }
        public string? FlexibilityNotes { get; set; }
        public string? SpecialRequirements { get; set; }
        public string? Notes { get; set; }
        public bool ConvertedToAllotment { get; set; }
        public string? AllotmentId { get; set; }
        public DateTime? ConversionDate { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string? CancelledBy { get; set; }
        public string? AssignedTo { get; set; }
        public string AssignedToName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int DaysWaiting { get; set; }
        public bool IsEligibleForNotification { get; set; }
    }

    public class BrandWaitingDto
    {
        public string Brand { get; set; } = string.Empty;
        public int TotalEntries { get; set; }
        public int ActiveEntries { get; set; }
        public int HighPriorityEntries { get; set; }
        public int MediumPriorityEntries { get; set; }
        public int LowPriorityEntries { get; set; }
        public int ConvertedEntries { get; set; }
        public decimal AverageWaitingDays { get; set; }
        public decimal ConversionRate { get; set; }
    }

    public class ModelWaitingDto
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int TotalEntries { get; set; }
        public int ActiveEntries { get; set; }
        public int HighPriorityEntries { get; set; }
        public int MediumPriorityEntries { get; set; }
        public int LowPriorityEntries { get; set; }
        public int ConvertedEntries { get; set; }
        public decimal AverageWaitingDays { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal AverageMaxPrice { get; set; }
        public decimal AverageMinPrice { get; set; }
    }

    public class PriorityWaitingDto
    {
        public int Priority { get; set; }
        public string PriorityName { get; set; } = string.Empty;
        public int TotalEntries { get; set; }
        public int ActiveEntries { get; set; }
        public int ConvertedEntries { get; set; }
        public decimal AverageWaitingDays { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal AverageMaxPrice { get; set; }
        public decimal AverageMinPrice { get; set; }
    }

    public class MonthlyWaitingDto
    {
        public string Month { get; set; } = string.Empty;
        public int NewEntries { get; set; }
        public int ConvertedEntries { get; set; }
        public int CancelledEntries { get; set; }
        public int ExpiredEntries { get; set; }
        public int ActiveEntries { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal AverageWaitingDays { get; set; }
    }
}
