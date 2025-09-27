using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.SalesOrders.Commands.CreateSalesOrder;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for sales order management - core business operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new sales order - main business process
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateSalesOrder([FromBody] CreateSalesOrderRequest request)
        {
            var command = new CreateSalesOrderCommand(
                request.OrderNumber,
                request.CustomerId,
                request.VehicleId,
                request.SalesPersonId,
                request.TotalAmount,
                request.PaymentMethod);

            var salesOrderId = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(GetSalesOrder), new { id = salesOrderId }, 
                new { id = salesOrderId, message = "Sales order created successfully" });
        }

        /// <summary>
        /// Gets a sales order by ID
        /// </summary>
        [HttpGet("{id}")]
        public Task<IActionResult> GetSalesOrder(string id)
        {
            // TODO: Implement GetSalesOrderByIdQuery when needed
            return Task.FromResult<IActionResult>(Ok(new { message = "Sales order retrieval not implemented yet" }));
        }
    }

    /// <summary>
    /// Request model for creating a sales order
    /// </summary>
    public class CreateSalesOrderRequest
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string SalesPersonId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}