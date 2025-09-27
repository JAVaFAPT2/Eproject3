using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for service order management
    /// </summary>
    [ApiController]
    [Route("api/service-orders")]
    [Authorize]
    public class ServiceOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiceOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all service orders with pagination and filters
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetServiceOrders(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] ServiceOrderStatus? status = null,
            [FromQuery] ServiceType? serviceType = null,
            [FromQuery] string? vehicleId = null,
            [FromQuery] string? customerId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            await Task.CompletedTask;
            return Ok(new { message = "Service orders endpoint ready", pageNumber, pageSize });
        }

        /// <summary>
        /// Gets a service order by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetServiceOrder(string id)
        {
            await Task.CompletedTask;
            return Ok(new { message = $"Service order {id} endpoint ready" });
        }

        /// <summary>
        /// Creates a new service order
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateServiceOrder([FromBody] object request)
        {
            await Task.CompletedTask;
            return Ok(new { message = "Service order created successfully" });
        }

        /// <summary>
        /// Starts a service order
        /// </summary>
        [HttpPut("{id}/start")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> StartServiceOrder(string id, [FromBody] object request)
        {
            await Task.CompletedTask;
            return Ok(new { message = "Service order started successfully" });
        }
    }
}