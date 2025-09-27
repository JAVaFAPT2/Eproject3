using MediatR;
using VehicleShowroomManagement.Application.Reports.DTOs;
using VehicleShowroomManagement.Application.Reports.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.Reports.Handlers
{
    /// <summary>
    /// Handler for getting customer information report
    /// </summary>
    public class GetCustomerInfoReportQueryHandler : IRequestHandler<GetCustomerInfoReportQuery, CustomerInfoReportDto>
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<SalesOrder> _salesOrderRepository;

        public GetCustomerInfoReportQueryHandler(
            IRepository<Customer> customerRepository,
            IRepository<SalesOrder> salesOrderRepository)
        {
            _customerRepository = customerRepository;
            _salesOrderRepository = salesOrderRepository;
        }

        public async Task<CustomerInfoReportDto> Handle(GetCustomerInfoReportQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllAsync();
            var salesOrders = await _salesOrderRepository.GetAllAsync();

            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                customers = customers.Where(c => 
                    c.FirstName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.LastName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.Phone.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.City))
            {
                customers = customers.Where(c => c.City == request.City);
            }

            if (!string.IsNullOrEmpty(request.State))
            {
                customers = customers.Where(c => c.State == request.State);
            }

            if (request.FromDate.HasValue)
            {
                customers = customers.Where(c => c.CreatedAt >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                customers = customers.Where(c => c.CreatedAt <= request.ToDate.Value);
            }

            var customerList = customers.ToList();
            var salesOrderList = salesOrders.ToList();

            var report = new CustomerInfoReportDto
            {
                GeneratedAt = DateTime.UtcNow,
                TotalCustomers = customerList.Count,
                ActiveCustomers = customerList.Count(c => salesOrderList.Any(so => so.CustomerId == c.Id)),
                NewCustomersThisMonth = customerList.Count(c => c.CreatedAt >= DateTime.UtcNow.AddMonths(-1)),
                TotalRevenue = salesOrderList.Sum(so => so.TotalAmount),
                AverageOrderValue = salesOrderList.Any() ? salesOrderList.Average(so => so.TotalAmount) : 0
            };

            // Generate customer details
            report.Customers = customerList.Select(customer =>
            {
                var customerOrders = salesOrderList.Where(so => so.CustomerId == customer.Id).ToList();
                return new CustomerDetailDto
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address,
                    City = customer.City,
                    State = customer.State,
                    ZipCode = customer.ZipCode,
                    Cccd = customer.Cccd,
                    CreatedAt = customer.CreatedAt,
                    UpdatedAt = customer.UpdatedAt,
                    TotalOrders = customerOrders.Count,
                    TotalSpent = customerOrders.Sum(so => so.TotalAmount),
                    LastOrderDate = customerOrders.Any() ? customerOrders.Max(so => so.OrderDate) : null,
                    Orders = request.IncludeOrderHistory ? customerOrders.Select(so => new CustomerOrderDto
                    {
                        Id = so.Id,
                        OrderNumber = $"ORD-{so.OrderDate:yyyyMMdd}-{so.Id[..8].ToUpper()}",
                        VehicleName = "Vehicle Name", // TODO: Get from vehicle repository
                        VehicleBrand = "Vehicle Brand", // TODO: Get from vehicle repository
                        TotalAmount = so.TotalAmount,
                        Status = so.Status,
                        OrderDate = so.OrderDate
                    }).ToList() : new List<CustomerOrderDto>()
                };
            }).OrderByDescending(c => c.TotalSpent).ToList();

            // Generate city-wise customer data
            report.CustomersByCity = customerList
                .GroupBy(c => c.City)
                .Select(g => new CityCustomerDto
                {
                    City = g.Key,
                    CustomerCount = g.Count(),
                    TotalRevenue = g.SelectMany(c => salesOrderList.Where(so => so.CustomerId == c.Id)).Sum(so => so.TotalAmount),
                    AverageOrderValue = g.SelectMany(c => salesOrderList.Where(so => so.CustomerId == c.Id)).Any() 
                        ? g.SelectMany(c => salesOrderList.Where(so => so.CustomerId == c.Id)).Average(so => so.TotalAmount) 
                        : 0
                })
                .OrderByDescending(c => c.CustomerCount)
                .ToList();

            // Generate state-wise customer data
            report.CustomersByState = customerList
                .GroupBy(c => c.State)
                .Select(g => new StateCustomerDto
                {
                    State = g.Key,
                    CustomerCount = g.Count(),
                    TotalRevenue = g.SelectMany(c => salesOrderList.Where(so => so.CustomerId == c.Id)).Sum(so => so.TotalAmount),
                    AverageOrderValue = g.SelectMany(c => salesOrderList.Where(so => so.CustomerId == c.Id)).Any() 
                        ? g.SelectMany(c => salesOrderList.Where(so => so.CustomerId == c.Id)).Average(so => so.TotalAmount) 
                        : 0
                })
                .OrderByDescending(s => s.CustomerCount)
                .ToList();

            // Generate monthly customer trends
            report.MonthlyCustomerTrends = customerList
                .GroupBy(c => new { Year = c.CreatedAt.Year, Month = c.CreatedAt.Month })
                .Select(g => new MonthlyCustomerDto
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    NewCustomers = g.Count(),
                    TotalCustomers = customerList.Count(c => c.CreatedAt <= new DateTime(g.Key.Year, g.Key.Month, DateTime.DaysInMonth(g.Key.Year, g.Key.Month))),
                    Revenue = salesOrderList.Where(so => so.OrderDate.Year == g.Key.Year && so.OrderDate.Month == g.Key.Month).Sum(so => so.TotalAmount)
                })
                .OrderBy(m => m.Month)
                .ToList();

            return report;
        }
    }
}
