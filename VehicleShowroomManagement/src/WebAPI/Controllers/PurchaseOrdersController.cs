using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder;
using VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.ApprovePurchaseOrder;
using VehicleShowroomManagement.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;
using VehicleShowroomManagement.Application.Features.PurchaseOrders.Queries.GetPurchaseOrders;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for purchase order management
    /// </summary>
    [ApiController]
    [Route("api/purchase-orders")]
    [Authorize]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PurchaseOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all purchase orders with pagination and filters
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetPurchaseOrders(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] PurchaseOrderStatus? status = null,
            [FromQuery] string? supplierId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetPurchaseOrdersQuery(pageNumber, pageSize, status, supplierId, fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets a purchase order by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetPurchaseOrder(string id)
        {
            var query = new GetPurchaseOrderByIdQuery(id);
            var order = await _mediator.Send(query);

            if (order == null)
                return NotFound(new { message = "Purchase order not found" });

            return Ok(order);
        }

        /// <summary>
        /// Creates a new purchase order
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderRequest request)
        {
            var command = new CreatePurchaseOrderCommand(
                request.OrderNumber,
                request.SupplierId,
                request.OrderDate,
                request.ExpectedDeliveryDate,
                request.OrderLines);

            var orderId = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(GetPurchaseOrder), new { id = orderId }, 
                new { id = orderId, message = "Purchase order created successfully" });
        }

        /// <summary>
        /// Approves a purchase order
        /// </summary>
        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApprovePurchaseOrder(string id, [FromBody] ApprovePurchaseOrderRequest request)
        {
            var command = new ApprovePurchaseOrderCommand(id, request.ApprovedBy, request.ApprovalNotes);
            await _mediator.Send(command);

            return Ok(new { message = "Purchase order approved successfully" });
        }
    }

    /// <summary>
    /// Request model for creating a purchase order
    /// </summary>
    public class CreatePurchaseOrderRequest
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string SupplierId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public List<PurchaseOrderLineRequest> OrderLines { get; set; } = new List<PurchaseOrderLineRequest>();
    }

    /// <summary>
    /// Request model for purchase order line items
    /// </summary>
    public class PurchaseOrderLineRequest
    {
        public string VehicleModelId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
    }

    /// <summary>
    /// Request model for approving purchase order
    /// </summary>
    public class ApprovePurchaseOrderRequest
    {
        public string ApprovedBy { get; set; } = string.Empty;
        public string? ApprovalNotes { get; set; }
    }
}