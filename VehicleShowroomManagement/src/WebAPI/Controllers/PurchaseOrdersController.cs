using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder;
using VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.UpdatePurchaseOrderStatus;
using VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.DeletePurchaseOrder;
using VehicleShowroomManagement.Application.Features.PurchaseOrders.Queries.GetPurchaseOrders;
using VehicleShowroomManagement.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;
using VehicleShowroomManagement.Application.Features.PurchaseOrderLines.Commands.AddPurchaseOrderLine;
using VehicleShowroomManagement.Application.Features.PurchaseOrderLines.Commands.DeletePurchaseOrderLine;
using VehicleShowroomManagement.Application.Features.PurchaseOrderLines.Commands.UpdatePurchaseOrderLine;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Dealer,Admin")]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PurchaseOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

    /// <summary>
    /// Gets all purchase orders with pagination
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPurchaseOrders(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? status = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        PurchaseOrderStatus? statusEnum = null;
        if (status.HasValue)
        {
            // Map numeric status to enum: 1=Pending, 2=Completed, 3=Cancelled
            statusEnum = status.Value switch
            {
                1 => PurchaseOrderStatus.Pending,
                2 => PurchaseOrderStatus.Completed,
                3 => PurchaseOrderStatus.Cancelled,
                _ => null
            };
        }

        var query = new GetPurchaseOrdersQuery(pageNumber, pageSize, statusEnum, fromDate, toDate);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

        /// <summary>
        /// Gets a single purchase order by ID with lines included
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseOrderById(string id)
        {
            var query = new GetPurchaseOrderByIdQuery(id);
            var result = await _mediator.Send(query);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Creates a new purchase order
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderRequest request)
        {
            var command = new CreatePurchaseOrderCommand(
                request.CreatedBy,
                request.TotalAmount);

            var poId = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreatePurchaseOrder), new { id = poId }, 
                new { id = poId, message = "Purchase order created successfully" });
        }

        [HttpPost("{id}/lines")]
        public async Task<IActionResult> AddPurchaseOrderLine(string id, [FromBody] AddPurchaseOrderLineRequest request)
        {
            var command = new AddPurchaseOrderLineCommand(
                id,
                request.ModelId,
                request.Quantity,
                request.PricePerUnit);

            var lineId = await _mediator.Send(command);
            return Ok(new { id = lineId, message = "Purchase order line added successfully" });
        }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdatePurchaseOrderStatus(string id, [FromBody] UpdatePurchaseOrderStatusRequest request)
    {
        await _mediator.Send(new UpdatePurchaseOrderStatusCommand(id, request.Status));
        return Ok(new { message = "Purchase order status updated" });
    }

        /// <summary>
        /// Deletes a purchase order and all its lines
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseOrder(string id)
        {
            await _mediator.Send(new DeletePurchaseOrderCommand(id));
            return Ok(new { message = "Purchase order deleted successfully" });
        }

        /// <summary>
        /// Deletes a single purchase order line
        /// </summary>
        [HttpDelete("{poId}/lines/{lineId}")]
        public async Task<IActionResult> DeletePurchaseOrderLine(string poId, string lineId)
        {
            await _mediator.Send(new DeletePurchaseOrderLineCommand(lineId));
            return Ok(new { message = "Purchase order line deleted successfully" });
        }

        /// <summary>
        /// Updates a purchase order line
        /// </summary>
        [HttpPut("{poId}/lines/{lineId}")]
        public async Task<IActionResult> UpdatePurchaseOrderLine(string poId, string lineId, [FromBody] UpdatePurchaseOrderLineRequest request)
        {
            await _mediator.Send(new UpdatePurchaseOrderLineCommand(lineId, request.Quantity, request.PricePerUnit));
            return Ok(new { message = "Purchase order line updated successfully" });
        }
    }

        public class CreatePurchaseOrderRequest
    {
        public string CreatedBy { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }

        public class AddPurchaseOrderLineRequest
    {
            public string ModelId { get; set; } = string.Empty; // Level-2 model (modelNumber)
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
    }

    public class UpdatePurchaseOrderStatusRequest
    {
        public PurchaseOrderStatus Status { get; set; }
    }

    public class UpdatePurchaseOrderLineRequest
    {
        public int? Quantity { get; set; }
        public decimal? PricePerUnit { get; set; }
    }
}
