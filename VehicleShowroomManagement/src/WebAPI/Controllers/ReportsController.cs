using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetCustomerAnalytics;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetTopVehicles;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetRecentOrders;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for reporting operations
    /// </summary>
    [ApiController]
    [Route("api/reports")]
    [Authorize]
    public class ReportsController(IMediator mediator) : ControllerBase
    {

        /// <summary>
        /// Gets stock availability report using dashboard analytics
        /// </summary>
        [HttpGet("stock-availability")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetStockAvailabilityReport([FromQuery] string? brand = null, [FromQuery] string? model = null, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            // Use existing dashboard queries to generate stock report
            var topVehiclesQuery = new GetTopVehiclesQuery(10, fromDate, toDate);
            var topVehicles = await mediator.Send(topVehiclesQuery);
            
            var stockReport = new
            {
                filters = new { brand, model, fromDate, toDate },
                stockData = topVehicles,
                generatedAt = DateTime.UtcNow,
                summary = new
                {
                    totalVehicles = topVehicles.Count,
                    totalValue = topVehicles.Sum(v => v.TotalRevenue)
                }
            };
            
            return Ok(stockReport);
        }

        /// <summary>
        /// Gets customer information report using dashboard analytics
        /// </summary>
        [HttpGet("customer-info")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetCustomerInfoReport([FromQuery] string? searchTerm = null, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            var customerAnalyticsQuery = new GetCustomerAnalyticsQuery(fromDate, toDate);
            var customerData = await mediator.Send(customerAnalyticsQuery);
            
            var customerReport = new
            {
                filters = new { searchTerm, fromDate, toDate },
                customerData,
                generatedAt = DateTime.UtcNow,
                summary = new
                {
                    totalCustomers = customerData.TotalCustomers,
                    newCustomersThisMonth = customerData.NewCustomers,
                    averageOrderValue = customerData.AverageCustomerValue
                }
            };
            
            return Ok(customerReport);
        }

        /// <summary>
        /// Gets vehicle master report using dashboard analytics
        /// </summary>
        [HttpGet("vehicle-master")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetVehicleMasterReport([FromQuery] string? brand = null, [FromQuery] string? model = null, [FromQuery] int? year = null)
        {
            var topVehiclesQuery = new GetTopVehiclesQuery(10, null, null);
            var vehiclesData = await mediator.Send(topVehiclesQuery);
            
            var vehicleReport = new
            {
                filters = new { brand, model, year },
                vehiclesData,
                generatedAt = DateTime.UtcNow,
                summary = new
                {
                    totalVehicles = vehiclesData.Count,
                    totalRevenue = vehiclesData.Sum(v => v.TotalRevenue),
                    averageSoldCount = vehiclesData.Average(v => v.SalesCount)
                }
            };
            
            return Ok(vehicleReport);
        }

        /// <summary>
        /// Gets allotment details report using recent orders
        /// </summary>
        [HttpGet("allotment-details")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetAllotmentDetailsReport([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null, [FromQuery] string? customerId = null)
        {
            var recentOrdersQuery = new GetRecentOrdersQuery(100);
            var ordersData = await mediator.Send(recentOrdersQuery);
            
            var allotmentReport = new
            {
                filters = new { fromDate, toDate, customerId },
                allotmentData = ordersData,
                generatedAt = DateTime.UtcNow,
                summary = new
                {
                    totalAllotments = ordersData.Count,
                    totalValue = ordersData.Sum(o => o.TotalAmount),
                    completedAllotments = ordersData.Count(o => o.Status == "Completed")
                }
            };
            
            return Ok(allotmentReport);
        }

        /// <summary>
        /// Gets waiting list report using recent orders with Waiting status
        /// </summary>
        [HttpGet("waiting-list")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetWaitingListReport([FromQuery] string? modelId = null, [FromQuery] string? customerId = null)
        {
            var recentOrdersQuery = new GetRecentOrdersQuery(100);
            var ordersData = await mediator.Send(recentOrdersQuery);
            
            // Filter for waiting orders
            var waitingOrders = ordersData.Where(o => o.Status == "Waiting").ToList();
            
            var waitingListReport = new
            {
                filters = new { modelId, customerId },
                waitingListData = waitingOrders,
                generatedAt = DateTime.UtcNow,
                summary = new
                {
                    totalWaiting = waitingOrders.Count,
                    totalValue = waitingOrders.Sum(o => o.TotalAmount),
                    oldestWaitingDate = waitingOrders.Any() ? waitingOrders.Min(o => o.OrderDate) : (DateTime?)null
                }
            };
            
            return Ok(waitingListReport);
        }

        /// <summary>
        /// Exports stock availability report to Excel
        /// </summary>
        [HttpGet("export/stock-availability")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportStockAvailability([FromQuery] string? brand = null, [FromQuery] string? model = null, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            var topVehiclesQuery = new GetTopVehiclesQuery(10, fromDate, toDate);
            var vehiclesData = await mediator.Send(topVehiclesQuery);
            
            // Generate Excel content (placeholder - would use actual Excel generation service)
            var reportData = new
            {
                filters = new { brand, model, fromDate, toDate },
                vehicles = vehiclesData,
                generatedAt = DateTime.UtcNow
            };
            
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(reportData);
            var content = System.Text.Encoding.UTF8.GetBytes(jsonContent);
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"stock-availability-{DateTime.UtcNow:yyyyMMdd}.xlsx");
        }

        [HttpGet("export/customer-info")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportCustomerInfo([FromQuery] string? searchTerm = null, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            await Task.CompletedTask;
            var content = System.Text.Encoding.UTF8.GetBytes("Sample Excel Content");
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "customer-info.xlsx");
        }

        [HttpGet("export/vehicle-master")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportVehicleMaster([FromQuery] string? brand = null, [FromQuery] string? model = null, [FromQuery] int? year = null)
        {
            await Task.CompletedTask;
            var content = System.Text.Encoding.UTF8.GetBytes("Sample Excel Content");
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "vehicle-master.xlsx");
        }

        [HttpGet("export/allotment-details")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportAllotmentDetails([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null, [FromQuery] string? customerId = null)
        {
            await Task.CompletedTask;
            var content = System.Text.Encoding.UTF8.GetBytes("Sample Excel Content");
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "allotment-details.xlsx");
        }

        [HttpGet("export/waiting-list")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportWaitingList([FromQuery] string? modelId = null, [FromQuery] string? customerId = null)
        {
            await Task.CompletedTask;
            var content = System.Text.Encoding.UTF8.GetBytes("Sample Excel Content");
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "waiting-list.xlsx");
        }
    }
}