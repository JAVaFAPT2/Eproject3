using System;
using System.Collections.Generic;

namespace VehicleShowroomManagement.Application.DTOs
{
    /// <summary>
    /// DTO for revenue comparison data
    /// </summary>
    public class RevenueComparisonDto
    {
        public List<RevenueData> CurrentPeriod { get; set; } = new List<RevenueData>();
        public List<RevenueData> PreviousPeriod { get; set; } = new List<RevenueData>();
        public decimal TotalCurrent { get; set; }
        public decimal TotalPrevious { get; set; }
        public decimal GrowthPercentage { get; set; }
    }

    /// <summary>
    /// Revenue data point
    /// </summary>
    public class RevenueData
    {
        public string Period { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int OrderCount { get; set; }
    }

    /// <summary>
    /// DTO for customer growth data
    /// </summary>
    public class CustomerGrowthDto
    {
        public List<CustomerGrowthData> CurrentPeriod { get; set; } = new List<CustomerGrowthData>();
        public List<CustomerGrowthData> PreviousPeriod { get; set; } = new List<CustomerGrowthData>();
        public int TotalCurrent { get; set; }
        public int TotalPrevious { get; set; }
        public decimal GrowthPercentage { get; set; }
    }

    /// <summary>
    /// Customer growth data point
    /// </summary>
    public class CustomerGrowthData
    {
        public string Period { get; set; } = string.Empty;
        public int CustomerCount { get; set; }
    }

    /// <summary>
    /// DTO for vehicle sales data
    /// </summary>
    public class VehicleSalesDto
    {
        public string VehicleId { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public int SalesCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AveragePrice { get; set; }
    }
}