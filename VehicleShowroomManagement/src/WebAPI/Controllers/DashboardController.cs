using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetRevenueAnalytics;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetCustomerAnalytics;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetTopVehicles;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetRecentOrders;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for dashboard analytics
    /// </summary>
    [ApiController]
    [Route("api/dashboard")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets revenue analytics for dashboard
        /// </summary>
        [HttpGet("revenue")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetRevenueAnalytics(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] string? period = "monthly")
        {
            var query = new GetRevenueAnalyticsQuery(fromDate, toDate, period);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets customer analytics for dashboard
        /// </summary>
        [HttpGet("customer")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetCustomerAnalytics(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetCustomerAnalyticsQuery(fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets top selling vehicles for dashboard
        /// </summary>
        [HttpGet("top-vehicles")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetTopVehicles(
            [FromQuery] int top = 10,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetTopVehiclesQuery(top, fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets recent orders for dashboard
        /// </summary>
        [HttpGet("recent-orders")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetRecentOrders(
            [FromQuery] int limit = 10)
        {
            var query = new GetRecentOrdersQuery(limit);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}