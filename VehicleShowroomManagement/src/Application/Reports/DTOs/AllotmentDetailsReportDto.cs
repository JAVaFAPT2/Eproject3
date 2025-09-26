namespace VehicleShowroomManagement.Application.Reports.DTOs
{
    /// <summary>
    /// Data Transfer Object for Allotment Details Report
    /// </summary>
    public class AllotmentDetailsReportDto
    {
        public DateTime GeneratedAt { get; set; }
        public int TotalAllotments { get; set; }
        public int ActiveAllotments { get; set; }
        public int ExpiredAllotments { get; set; }
        public int ConvertedAllotments { get; set; }
        public int CancelledAllotments { get; set; }
        public decimal TotalReservationAmount { get; set; }
        public decimal CollectedReservationAmount { get; set; }
        public decimal PendingReservationAmount { get; set; }
        public List<AllotmentDetailDto> Allotments { get; set; } = new List<AllotmentDetailDto>();
        public List<StatusSummaryDto> StatusSummaries { get; set; } = new List<StatusSummaryDto>();
        public List<TypeSummaryDto> TypeSummaries { get; set; } = new List<TypeSummaryDto>();
        public List<MonthlyAllotmentDto> MonthlyTrends { get; set; } = new List<MonthlyAllotmentDto>();
    }

    public class AllotmentDetailDto
    {
        public string Id { get; set; } = string.Empty;
        public string AllotmentNumber { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string VehicleName { get; set; } = string.Empty;
        public string VehicleBrand { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string SalesPersonId { get; set; } = string.Empty;
        public string SalesPersonName { get; set; } = string.Empty;
        public DateTime AllotmentDate { get; set; }
        public DateTime ValidUntil { get; set; }
        public string Status { get; set; } = string.Empty;
        public string AllotmentType { get; set; } = string.Empty;
        public int Priority { get; set; }
        public decimal ReservationAmount { get; set; }
        public bool ReservationPaid { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentReference { get; set; }
        public string? SpecialConditions { get; set; }
        public string? Notes { get; set; }
        public bool ConvertedToOrder { get; set; }
        public string? OrderId { get; set; }
        public DateTime? ConversionDate { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string? CancelledBy { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsExpired { get; set; }
        public int DaysRemaining { get; set; }
    }

    public class StatusSummaryDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalReservationAmount { get; set; }
        public decimal CollectedAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal Percentage { get; set; }
    }

    public class TypeSummaryDto
    {
        public string AllotmentType { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalReservationAmount { get; set; }
        public decimal CollectedAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal AverageReservationAmount { get; set; }
    }

    public class MonthlyAllotmentDto
    {
        public string Month { get; set; } = string.Empty;
        public int NewAllotments { get; set; }
        public int ConvertedAllotments { get; set; }
        public int ExpiredAllotments { get; set; }
        public int CancelledAllotments { get; set; }
        public decimal TotalReservationAmount { get; set; }
        public decimal CollectedAmount { get; set; }
        public decimal ConversionRate { get; set; }
    }
}
