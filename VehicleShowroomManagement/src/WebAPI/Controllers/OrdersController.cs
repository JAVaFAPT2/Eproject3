using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Orders.Commands.CreateOrder;
using VehicleShowroomManagement.Application.Features.Orders.Commands.AssignVehicle;
using VehicleShowroomManagement.Application.Features.Orders.Commands.UpdateOrderStatus;
using VehicleShowroomManagement.Application.Features.Orders.Queries.GetOrders;
using VehicleShowroomManagement.Application.Features.Orders.Queries.GetOrderById;
using VehicleShowroomManagement.Domain.Enums;

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
        [Authorize(Roles = "Dealer,Admin,Customer")]
        public async Task<IActionResult> GetOrders(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? status = null,
            [FromQuery] string? customerId = null)
        {
            OrderStatus? statusEnum = null;
            if (status.HasValue)
            {
                // 1=Pending, 2=Confirmed, 3=Completed, 4=Cancelled
                statusEnum = status.Value switch
                {
                    1 => OrderStatus.Pending,
                    2 => OrderStatus.Confirmed,
                    3 => OrderStatus.Completed,
                    4 => OrderStatus.Cancelled,
                    _ => null
                };
            }

            var query = new GetOrdersQuery(pageNumber, pageSize, statusEnum?.ToString(), customerId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets a single order by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var query = new GetOrderByIdQuery(id);
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound(new { message = "Order not found" });
                
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
            var command = new AssignVehicleCommand(id, request.VehicleId, request.DealerId);
            await _mediator.Send(command);
            return Ok(new { message = "Vehicle assigned successfully" });
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateOrderStatus(string id, [FromBody] UpdateOrderStatusRequest request)
        {
            await _mediator.Send(new UpdateOrderStatusCommand(id, request.Status));
            return Ok(new { message = "Order status updated" });
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
        public string DealerId { get; set; } = string.Empty;
    }

    public class UpdateOrderStatusRequest
    {
        public OrderStatus Status { get; set; }
    }
}

