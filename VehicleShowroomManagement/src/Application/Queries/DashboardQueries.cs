using System.Collections.Generic;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;

namespace VehicleShowroomManagement.Application.Queries
{
    /// <summary>
    /// Query for revenue comparison by time period
    /// </summary>
    public class GetRevenueComparisonQuery : IRequest<RevenueComparisonDto>
    {
        public string Period { get; set; }

        public GetRevenueComparisonQuery(string period)
        {
            Period = period;
        }
    }

    /// <summary>
    /// Query for customer growth by time period
    /// </summary>
    public class GetCustomerGrowthQuery : IRequest<CustomerGrowthDto>
    {
        public string Period { get; set; }

        public GetCustomerGrowthQuery(string period)
        {
            Period = period;
        }
    }

    /// <summary>
    /// Query for top 5 best-selling vehicles
    /// </summary>
    public class GetTopVehiclesQuery : IRequest<IEnumerable<VehicleSalesDto>>
    {
    }

    /// <summary>
    /// Query for recent orders (5 most recent)
    /// </summary>
    public class GetRecentOrdersQuery : IRequest<IEnumerable<OrderDto>>
    {
    }
}