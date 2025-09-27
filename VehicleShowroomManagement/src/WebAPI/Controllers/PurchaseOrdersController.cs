using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.PurchaseOrders.Commands;
using VehicleShowroomManagement.Application.PurchaseOrders.Queries;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing purchase orders
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PurchaseOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all purchase orders with filtering and pagination
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetPurchaseOrders(
            [FromQuery] string? searchTerm,
            [FromQuery] string? status,
            [FromQuery] string? brand,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetPurchaseOrdersQuery
            {
                SearchTerm = searchTerm,
                Status = status,
                Brand = brand,
                FromDate = fromDate,
                ToDate = toDate,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get purchase order by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetPurchaseOrder(string id)
        {
            var query = new GetPurchaseOrderByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Create new purchase order
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPurchaseOrder), new { id = result }, result);
        }

        /// <summary>
        /// Update purchase order
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdatePurchaseOrder(string id, [FromBody] UpdatePurchaseOrderCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return Ok(new { message = "Purchase order updated successfully" });
        }

        /// <summary>
        /// Delete purchase order
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> DeletePurchaseOrder(string id)
        {
            var command = new DeletePurchaseOrderCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return Ok(new { message = "Purchase order deleted successfully" });
        }

        /// <summary>
        /// Approve purchase order
        /// </summary>
        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApprovePurchaseOrder(string id)
        {
            var command = new ApprovePurchaseOrderCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest(new { message = "Purchase order cannot be approved" });

            return Ok(new { message = "Purchase order approved successfully" });
        }
    }
}
