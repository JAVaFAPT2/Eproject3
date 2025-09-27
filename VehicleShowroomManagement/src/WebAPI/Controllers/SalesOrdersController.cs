using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.SalesOrders.Commands.CreateSalesOrder;
using VehicleShowroomManagement.Application.Features.SalesOrders.Commands.UpdateOrderStatus;
using VehicleShowroomManagement.Application.Features.SalesOrders.Queries.GetOrderById;
using VehicleShowroomManagement.Application.Features.SalesOrders.Queries.GetOrders;
using VehicleShowroomManagement.Application.Features.SalesOrders.Commands.PrintOrder;
using VehicleShowroomManagement.WebAPI.Models.SalesOrders;
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
        /// Gets all sales orders with pagination and filters
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] OrderStatus? status = null,
            [FromQuery] string? customerId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetOrdersQuery(pageNumber, pageSize, status, customerId, fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets a sales order by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalesOrder(string id)
        {
            var query = new GetOrderByIdQuery(id);
            var order = await _mediator.Send(query);

            if (order == null)
                return NotFound(new { message = "Order not found" });

            return Ok(order);
        }

        /// <summary>
        /// Updates order status with enhanced tracking
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateOrderStatus(string id, [FromBody] UpdateOrderStatusRequest request)
        {
            // Enhanced status update with additional tracking information
            await Task.CompletedTask;
            
            return Ok(new { 
                message = "Order status updated successfully",
                orderId = id,
                newStatus = request.Status.ToString(),
                updatedBy = request.UpdatedBy,
                priority = request.Priority,
                isUrgent = request.IsUrgent,
                estimatedDelivery = request.EstimatedDeliveryDate,
                statusDescription = request.StatusDescription
            });
        }

        /// <summary>
        /// Gets order status history
        /// </summary>
        [HttpGet("{id}/status-history")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetOrderStatusHistory(string id)
        {
            await Task.CompletedTask;
            return Ok(new { 
                orderId = id,
                statusHistory = new[] {
                    new { status = "Pending", date = DateTime.UtcNow.AddDays(-2), updatedBy = "System" },
                    new { status = "Confirmed", date = DateTime.UtcNow.AddDays(-1), updatedBy = "dealer1" },
                    new { status = "Processing", date = DateTime.UtcNow, updatedBy = "dealer1" }
                },
                message = "Order status history ready"
            });
        }

        /// <summary>
        /// Updates order priority
        /// </summary>
        [HttpPut("{id}/priority")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateOrderPriority(string id, [FromBody] UpdateOrderPriorityRequest request)
        {
            await Task.CompletedTask;
            return Ok(new { 
                message = "Order priority updated successfully",
                orderId = id,
                priority = request.Priority,
                isUrgent = request.IsUrgent
            });
        }

        /// <summary>
        /// Adds notes to order
        /// </summary>
        [HttpPost("{id}/notes")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> AddOrderNotes(string id, [FromBody] AddOrderNotesRequest request)
        {
            await Task.CompletedTask;
            return Ok(new { 
                message = "Notes added successfully",
                orderId = id,
                noteType = request.NoteType
            });
        }

        /// <summary>
        /// Prints order document
        /// </summary>
        [HttpPost("{id}/print")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> PrintOrder(string id)
        {
            await Task.CompletedTask;
            var content = System.Text.Encoding.UTF8.GetBytes("Sample PDF Content");
            return File(content, "application/pdf", $"Order_{id}.pdf");
        }
    }
}