using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Application.Commands;
using VehicleShowroomManagement.Application.Queries;
using System.Threading.Tasks;
using System.Collections.Generic;

using CustomerInfo = VehicleShowroomManagement.Application.DTOs.CustomerInfo;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Orders controller for sales order management
    /// </summary>
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
        /// Get all orders
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = new GetOrdersQuery(pageNumber, pageSize);
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get order by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(string id)
        {
            try
            {
                var query = new GetOrderByIdQuery(id);
                var result = await _mediator.Send(query);

                if (result == null)
                {
                    return NotFound(new { message = "Order not found" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create new order (with customer)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var command = new CreateOrderCommand(
                    request.Customer,
                    request.VehicleId,
                    request.TotalAmount,
                    request.PaymentMethod);

                var result = await _mediator.Send(command);

                return CreatedAtAction(nameof(GetOrder), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Print order
        /// </summary>
        [HttpPost("{id}/print")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> PrintOrder(string id)
        {
            try
            {
                var command = new PrintOrderCommand(id);
                var result = await _mediator.Send(command);

                return Ok(new { message = "Order printed successfully", printData = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    /// <summary>
    /// Request model for creating an order
    /// </summary>
    public class CreateOrderRequest
    {
        public CustomerInfo Customer { get; set; } = new CustomerInfo();
        public string VehicleId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = "CASH";
    }
}