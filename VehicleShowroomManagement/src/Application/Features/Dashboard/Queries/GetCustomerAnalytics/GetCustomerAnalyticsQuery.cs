using MediatR;

namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetCustomerAnalytics
{
    /// <summary>
    /// Query for getting customer analytics
    /// </summary>
    public record GetCustomerAnalyticsQuery(DateTime? FromDate, DateTime? ToDate) : IRequest<CustomerAnalyticsResult>;

    /// <summary>
    /// Result for customer analytics query
    /// </summary>
    public class CustomerAnalyticsResult
    {
        public int TotalCustomers { get; set; }
        public int NewCustomers { get; set; }
        public int ActiveCustomers { get; set; }
        public decimal CustomerGrowthPercentage { get; set; }
        public List<CustomerDataPoint> CustomerGrowthData { get; set; } = new List<CustomerDataPoint>();
        public List<TopCustomer> TopCustomers { get; set; } = new List<TopCustomer>();
        public decimal AverageCustomerValue { get; set; }
        public int RepeatCustomers { get; set; }
    }

    /// <summary>
    /// Customer data point for charts
    /// </summary>
    public class CustomerDataPoint
    {
        public string Label { get; set; } = string.Empty;
        public int NewCustomers { get; set; }
        public int TotalCustomers { get; set; }
        public DateTime Date { get; set; }
    }

    /// <summary>
    /// Top customer information
    /// </summary>
    public class TopCustomer
    {
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }
        public int OrderCount { get; set; }
        public DateTime LastOrderDate { get; set; }
    }
}