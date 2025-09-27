using MediatR;

namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetRevenueAnalytics
{
    /// <summary>
    /// Query for getting revenue analytics
    /// </summary>
    public record GetRevenueAnalyticsQuery(
        DateTime? FromDate,
        DateTime? ToDate,
        string Period) : IRequest<RevenueAnalyticsResult>;

    /// <summary>
    /// Result for revenue analytics query
    /// </summary>
    public class RevenueAnalyticsResult
    {
        public decimal TotalRevenue { get; set; }
        public decimal PreviousPeriodRevenue { get; set; }
        public decimal GrowthPercentage { get; set; }
        public List<RevenueDataPoint> RevenueData { get; set; } = new List<RevenueDataPoint>();
        public List<RevenueByCategory> RevenueByCategory { get; set; } = new List<RevenueByCategory>();
        public decimal AverageOrderValue { get; set; }
        public int TotalOrders { get; set; }
    }

    /// <summary>
    /// Revenue data point for charts
    /// </summary>
    public class RevenueDataPoint
    {
        public string Label { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }

    /// <summary>
    /// Revenue breakdown by category
    /// </summary>
    public class RevenueByCategory
    {
        public string Category { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
        public decimal Percentage { get; set; }
    }
}