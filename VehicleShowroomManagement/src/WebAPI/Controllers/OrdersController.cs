using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Orders.Commands.CreateOrder;
using VehicleShowroomManagement.Application.Features.Orders.Commands.AssignVehicle;
using VehicleShowroomManagement.Application.Features.Orders.Commands.ConfirmOrder;
using VehicleShowroomManagement.Application.Features.Orders.Commands.CompleteOrder;
using VehicleShowroomManagement.Application.Features.Orders.Queries.GetOrders;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all orders with pagination
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? status = null,
            [FromQuery] string? customerId = null)
        {
            var query = new GetOrdersQuery(pageNumber, pageSize, status, customerId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var command = new CreateOrderCommand(
                request.CustomerId,
                request.ModelNumber,
                request.SalePrice);

            var orderId = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateOrder), new { id = orderId }, 
                new { id = orderId, message = "Order created successfully" });
        }

        [HttpPost("{id}/assign-vehicle")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> AssignVehicle(string id, [FromBody] AssignVehicleRequest request)
        {
            var command = new AssignVehicleCommand(id, request.VehicleId);
            await _mediator.Send(command);
            return Ok(new { message = "Vehicle assigned successfully" });
        }

        [HttpPost("{id}/confirm")]
        [Authorize(Roles = "Dealer,Admin,Customer")]
        public async Task<IActionResult> ConfirmOrder(string id)
        {
            var command = new ConfirmOrderCommand(id);
            await _mediator.Send(command);
            return Ok(new { message = "Order confirmed successfully" });
        }

        [HttpPost("{id}/complete")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CompleteOrder(string id)
        {
            var command = new CompleteOrderCommand(id);
            await _mediator.Send(command);
            return Ok(new { message = "Order completed successfully" });
        }
    }

    public class CreateOrderRequest
    {
        public string CustomerId { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public decimal SalePrice { get; set; }
    }

    public class AssignVehicleRequest
    {
        public string VehicleId { get; set; } = string.Empty;
    }
}

