using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Queries.GetServiceOrderById;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
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
        /// Creates a new service order
        /// </summary>
        /// <param name="command">Service order creation details</param>
        /// <returns>The ID of the created service order</returns>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateServiceOrder([FromBody] CreateServiceOrderCommand command)
        {
            try
            {
                var serviceOrderId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetServiceOrder), new { id = serviceOrderId }, new { Id = serviceOrderId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while creating the service order", Details = ex.Message });
            }
        }

        /// <summary>
        /// Gets a service order by ID
        /// </summary>
        /// <param name="id">Service order ID</param>
        /// <returns>Service order details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,HR,Admin")]
        public async Task<IActionResult> GetServiceOrder(string id)
        {
            try
            {
                var query = new GetServiceOrderByIdQuery { ServiceOrderId = id };
                var serviceOrder = await _mediator.Send(query);
                return Ok(serviceOrder);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while retrieving the service order", Details = ex.Message });
            }
        }
    }
}