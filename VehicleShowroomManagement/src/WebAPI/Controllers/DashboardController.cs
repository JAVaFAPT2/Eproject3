using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using VehicleShowroomManagement.Application.Queries;
using System.Threading.Tasks;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Dashboard controller for analytics and reporting
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
        /// Get revenue comparison by time period (month, year)
        /// </summary>
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueComparison(
            [FromQuery] string period = "month")
        {
            try
            {
                var query = new GetRevenueComparisonQuery(period);
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get customer growth by time period (month, year)
        /// </summary>
        [HttpGet("customer")]
        public async Task<IActionResult> GetCustomerGrowth(
            [FromQuery] string period = "month")
        {
            try
            {
                var query = new GetCustomerGrowthQuery(period);
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get top 5 best-selling vehicles
        /// </summary>
        [HttpGet("top-vehicles")]
        public async Task<IActionResult> GetTopVehicles()
        {
            try
            {
                var query = new GetTopVehiclesQuery();
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get recent orders (5 most recent)
        /// </summary>
        [HttpGet("recent-orders")]
        public async Task<IActionResult> GetRecentOrders()
        {
            try
            {
                var query = new GetRecentOrdersQuery();
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}