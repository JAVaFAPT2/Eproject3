using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Dashboard.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Dashboard.Handlers
{
    /// <summary>
    /// Handler for revenue comparison query
    /// </summary>
    public class GetRevenueComparisonQueryHandler : IRequestHandler<GetRevenueComparisonQuery, RevenueComparisonDto>
    {
        private readonly IRepository<SalesOrder> _orderRepository;

        public GetRevenueComparisonQueryHandler(IRepository<SalesOrder> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<RevenueComparisonDto> Handle(GetRevenueComparisonQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            DateTime currentStart, currentEnd, previousStart, previousEnd;

            switch (request.Period.ToLower())
            {
                case "month":
                    currentStart = new DateTime(now.Year, now.Month, 1);
                    currentEnd = currentStart.AddMonths(1).AddDays(-1);
                    previousStart = currentStart.AddMonths(-1);
                    previousEnd = previousStart.AddMonths(1).AddDays(-1);
                    break;
                case "year":
                    currentStart = new DateTime(now.Year, 1, 1);
                    currentEnd = new DateTime(now.Year, 12, 31);
                    previousStart = new DateTime(now.Year - 1, 1, 1);
                    previousEnd = new DateTime(now.Year - 1, 12, 31);
                    break;
                default:
                    throw new ArgumentException("Invalid period. Use 'month' or 'year'");
            }

            var allOrders = await _orderRepository.GetAllAsync();
            var orders = allOrders.Where(o => !o.IsDeleted && o.Status == "COMPLETED").ToList();

            var currentOrders = orders.Where(o =>
                o.OrderDate >= currentStart && o.OrderDate <= currentEnd).ToList();

            var previousOrders = orders.Where(o =>
                o.OrderDate >= previousStart && o.OrderDate <= previousEnd).ToList();

            var currentData = GetRevenueData(currentOrders, currentStart, currentEnd, request.Period);
            var previousData = GetRevenueData(previousOrders, previousStart, previousEnd, request.Period);

            var totalCurrent = currentOrders.Sum(o => o.SalePrice);
            var totalPrevious = previousOrders.Sum(o => o.SalePrice);
            var growthPercentage = totalPrevious > 0 ? ((totalCurrent - totalPrevious) / totalPrevious) * 100 : 0;

            return new RevenueComparisonDto
            {
                CurrentPeriod = currentData,
                PreviousPeriod = previousData,
                TotalCurrent = totalCurrent,
                TotalPrevious = totalPrevious,
                GrowthPercentage = growthPercentage
            };
        }

        private List<RevenueData> GetRevenueData(List<SalesOrder> orders, DateTime start, DateTime end, string period)
        {
            if (period.ToLower() == "month")
            {
                var days = Enumerable.Range(0, (end - start).Days + 1)
                    .Select(d => start.AddDays(d).Date)
                    .ToList();

                return days.Select(day => new RevenueData
                {
                    Period = day.ToString("dd/MM"),
                    Amount = orders.Where(o => o.OrderDate.Date == day).Sum(o => o.SalePrice),
                    OrderCount = orders.Count(o => o.OrderDate.Date == day)
                }).ToList();
            }
            else // year
            {
                var months = Enumerable.Range(1, 12).Select(m => new DateTime(start.Year, m, 1)).ToList();

                return months.Select(month => new RevenueData
                {
                    Period = month.ToString("MMM"),
                    Amount = orders.Where(o => o.OrderDate.Year == month.Year && o.OrderDate.Month == month.Month).Sum(o => o.SalePrice),
                    OrderCount = orders.Count(o => o.OrderDate.Year == month.Year && o.OrderDate.Month == month.Month)
                }).ToList();
            }
        }
    }

    /// <summary>
    /// Handler for customer growth query
    /// </summary>
    public class GetCustomerGrowthQueryHandler : IRequestHandler<GetCustomerGrowthQuery, CustomerGrowthDto>
    {
        private readonly IRepository<Customer> _customerRepository;

        public GetCustomerGrowthQueryHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerGrowthDto> Handle(GetCustomerGrowthQuery request, CancellationToken cancellationToken)
        {
            var allCustomers = await _customerRepository.GetAllAsync();
            var customers = allCustomers.Where(c => !c.IsDeleted).ToList();

            var now = DateTime.UtcNow;
            DateTime currentStart, previousStart;

            switch (request.Period.ToLower())
            {
                case "month":
                    currentStart = new DateTime(now.Year, now.Month, 1);
                    previousStart = currentStart.AddMonths(-1);
                    break;
                case "year":
                    currentStart = new DateTime(now.Year, 1, 1);
                    previousStart = new DateTime(now.Year - 1, 1, 1);
                    break;
                default:
                    throw new ArgumentException("Invalid period. Use 'month' or 'year'");
            }

            var currentCustomers = customers.Where(c => c.CreatedAt >= currentStart).ToList();
            var previousCustomers = customers.Where(c => c.CreatedAt >= previousStart && c.CreatedAt < currentStart).ToList();

            var currentData = GetCustomerGrowthData(currentCustomers, currentStart, request.Period);
            var previousData = GetCustomerGrowthData(previousCustomers, previousStart, request.Period);

            var totalCurrent = currentCustomers.Count;
            var totalPrevious = previousCustomers.Count;
            var growthPercentage = totalPrevious > 0 ? ((totalCurrent - totalPrevious) / (decimal)totalPrevious) * 100 : 0;

            return new CustomerGrowthDto
            {
                CurrentPeriod = currentData,
                PreviousPeriod = previousData,
                TotalCurrent = totalCurrent,
                TotalPrevious = totalPrevious,
                GrowthPercentage = growthPercentage
            };
        }

        private List<CustomerGrowthData> GetCustomerGrowthData(List<Customer> customers, DateTime start, string period)
        {
            if (period.ToLower() == "month")
            {
                var days = Enumerable.Range(0, DateTime.UtcNow.Day)
                    .Select(d => start.AddDays(d).Date)
                    .ToList();

                return days.Select(day => new CustomerGrowthData
                {
                    Period = day.ToString("dd/MM"),
                    CustomerCount = customers.Count(c => c.CreatedAt.Date <= day)
                }).ToList();
            }
            else // year
            {
                var months = Enumerable.Range(1, 12).Select(m => new DateTime(start.Year, m, 1)).ToList();

                return months.Select(month => new CustomerGrowthData
                {
                    Period = month.ToString("MMM"),
                    CustomerCount = customers.Count(c => c.CreatedAt.Year == month.Year && c.CreatedAt.Month <= month.Month)
                }).ToList();
            }
        }
    }

    /// <summary>
    /// Handler for top vehicles query
    /// </summary>
    public class GetTopVehiclesQueryHandler : IRequestHandler<GetTopVehiclesQuery, IEnumerable<VehicleSalesDto>>
    {
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<SalesOrderItem> _orderItemRepository;

        public GetTopVehiclesQueryHandler(
            IRepository<Vehicle> vehicleRepository,
            IRepository<SalesOrderItem> orderItemRepository)
        {
            _vehicleRepository = vehicleRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<IEnumerable<VehicleSalesDto>> Handle(GetTopVehiclesQuery request, CancellationToken cancellationToken)
        {
            var allVehicles = await _vehicleRepository.GetAllAsync();
            var vehicles = allVehicles.Where(v => !v.IsDeleted && v.Status == "SOLD").ToList();

            var allOrderItems = await _orderItemRepository.GetAllAsync();
            var orderItems = allOrderItems.Where(oi => !oi.IsDeleted).ToList();

            var vehicleSales = vehicles.GroupBy(v => v.Id)
                .Select(g =>
                {
                    var vehicle = g.First();
                    var salesCount = orderItems.Count(oi => oi.VehicleId == vehicle.Id);
                    var totalRevenue = g.Sum(v => v.PurchasePrice);

                    return new VehicleSalesDto
                    {
                        VehicleId = vehicle.Id,
                        VIN = vehicle.RegistrationData?.VIN ?? vehicle.VehicleId,
                        ModelName = vehicle.ModelNumber,
                        BrandName = "Unknown", // Not available in new schema
                        SalesCount = salesCount,
                        TotalRevenue = totalRevenue,
                        AveragePrice = salesCount > 0 ? totalRevenue / salesCount : 0
                    };
                })
                .OrderByDescending(v => v.SalesCount)
                .Take(5)
                .ToList();

            return vehicleSales;
        }
    }

    /// <summary>
    /// Handler for recent orders query
    /// </summary>
    public class GetRecentOrdersQueryHandler : IRequestHandler<GetRecentOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IRepository<SalesOrder> _orderRepository;

        public GetRecentOrdersQueryHandler(IRepository<SalesOrder> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetRecentOrdersQuery request, CancellationToken cancellationToken)
        {
            var allOrders = await _orderRepository.GetAllAsync();
            var orders = allOrders.Where(o => !o.IsDeleted)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToList();

            return orders.Select(MapToDto).ToList();
        }

        private static OrderDto MapToDto(SalesOrder order)
        {
            return new OrderDto
            {
                Id = order.Id.ToString(),
                OrderNumber = $"ORD-{order.OrderDate:yyyyMMdd}-{order.Id.ToString().Substring(0, 4)}",
                CustomerId = order.CustomerId,
                Customer = new CustomerInfo
                {
                    CustomerId = order.CustomerId,
                    Name = "Customer Name",
                    Email = "customer@example.com",
                    Phone = "0901234567",
                    Address = "Customer Address"
                },
                VehicleId = "VehicleId",
                Vehicle = new VehicleInfo
                {
                    VehicleId = "VehicleId",
                    VIN = "VIN123",
                    ModelNumber = "MODEL001",
                    Name = "Vehicle Name",
                    Brand = "Brand Name",
                    Price = 50000
                },
                SalesPersonId = order.EmployeeId,
                SalesPerson = new UserInfo
                {
                    UserId = order.EmployeeId,
                    Username = "employee",
                    FullName = "Employee",
                    Email = "employee@example.com"
                },
                Status = order.Status,
                TotalAmount = order.SalePrice,
                PaymentMethod = "CASH",
                OrderDate = order.OrderDate,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };
        }
    }
}
