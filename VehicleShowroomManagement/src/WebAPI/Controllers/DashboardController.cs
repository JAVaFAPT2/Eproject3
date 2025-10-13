using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetRevenueAnalytics;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetCustomerAnalytics;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetTopVehicles;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetRecentOrders;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetOverview;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for dashboard analytics and reporting
    /// </summary>
    [ApiController]
    [Route("api/dashboard")]
    [Authorize]
    public class DashboardController(IMediator mediator) : ControllerBase
    {
    /// <summary>
    /// Gets overview metrics
    /// </summary>
    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview()
    {
        var result = await mediator.Send(new GetOverviewQuery());
        return Ok(result);
    }
    /// <summary>
    /// Gets revenue analytics data
    /// </summary>
    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenueAnalytics([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null, [FromQuery] string period = "monthly")
    {
        var query = new GetRevenueAnalyticsQuery(fromDate, toDate, period);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets customer analytics data
    /// </summary>
    [HttpGet("customer")]
    public async Task<IActionResult> GetCustomerAnalytics([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        var query = new GetCustomerAnalyticsQuery(fromDate, toDate);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets top selling vehicles
    /// </summary>
    [HttpGet("top-vehicles")]
    public async Task<IActionResult> GetTopVehicles([FromQuery] int top = 10, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        var query = new GetTopVehiclesQuery(top, fromDate, toDate);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets recent orders for dashboard
    /// </summary>
    [HttpGet("recent-orders")]
    public async Task<IActionResult> GetRecentOrders([FromQuery] int limit = 10)
    {
        var query = new GetRecentOrdersQuery(limit);
        var result = await mediator.Send(query);
        return Ok(result);
    }
    }
}
