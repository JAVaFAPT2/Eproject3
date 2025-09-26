using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.ServiceOrders.Commands;
using VehicleShowroomManagement.Application.ServiceOrders.Queries;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing service orders
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServiceOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiceOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all service orders with filtering and pagination
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetServiceOrders(
            [FromQuery] string? searchTerm,
            [FromQuery] string? status,
            [FromQuery] string? serviceType,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetServiceOrdersQuery
            {
                SearchTerm = searchTerm,
                Status = status,
                ServiceType = serviceType,
                FromDate = fromDate,
                ToDate = toDate,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get service order by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetServiceOrder(string id)
        {
            var query = new GetServiceOrderByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Create new service order
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateServiceOrder([FromBody] CreateServiceOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetServiceOrder), new { id = result }, result);
        }

        /// <summary>
        /// Start service order
        /// </summary>
        [HttpPost("{id}/start")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> StartServiceOrder(string id)
        {
            var command = new StartServiceOrderCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest(new { message = "Service order cannot be started" });

            return Ok(new { message = "Service order started successfully" });
        }

        /// <summary>
        /// Complete service order
        /// </summary>
        [HttpPost("{id}/complete")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CompleteServiceOrder(string id, [FromBody] CompleteServiceOrderCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest(new { message = "Service order cannot be completed" });

            return Ok(new { message = "Service order completed successfully" });
        }

        /// <summary>
        /// Cancel service order
        /// </summary>
        [HttpPost("{id}/cancel")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CancelServiceOrder(string id, [FromBody] CancelServiceOrderCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest(new { message = "Service order cannot be cancelled" });

            return Ok(new { message = "Service order cancelled successfully" });
        }
    }
}
