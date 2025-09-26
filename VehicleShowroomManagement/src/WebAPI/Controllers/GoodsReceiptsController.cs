using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.GoodsReceipts.Commands;
using VehicleShowroomManagement.Application.GoodsReceipts.Queries;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing goods receipts
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GoodsReceiptsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GoodsReceiptsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all goods receipts with filtering and pagination
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetGoodsReceipts(
            [FromQuery] string? searchTerm,
            [FromQuery] string? status,
            [FromQuery] string? brand,
            [FromQuery] string? condition,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetGoodsReceiptsQuery
            {
                SearchTerm = searchTerm,
                Status = status,
                Brand = brand,
                Condition = condition,
                FromDate = fromDate,
                ToDate = toDate,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get goods receipt by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetGoodsReceipt(string id)
        {
            var query = new GetGoodsReceiptByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Create new goods receipt
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateGoodsReceipt([FromBody] CreateGoodsReceiptCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetGoodsReceipt), new { id = result }, result);
        }

        /// <summary>
        /// Inspect goods receipt
        /// </summary>
        [HttpPost("{id}/inspect")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> InspectGoodsReceipt(string id, [FromBody] InspectGoodsReceiptCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest(new { message = "Goods receipt cannot be inspected" });

            return Ok(new { message = "Goods receipt inspected successfully" });
        }

        /// <summary>
        /// Accept goods receipt
        /// </summary>
        [HttpPost("{id}/accept")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> AcceptGoodsReceipt(string id)
        {
            var command = new AcceptGoodsReceiptCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest(new { message = "Goods receipt cannot be accepted" });

            return Ok(new { message = "Goods receipt accepted successfully" });
        }

        /// <summary>
        /// Reject goods receipt
        /// </summary>
        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> RejectGoodsReceipt(string id, [FromBody] RejectGoodsReceiptCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest(new { message = "Goods receipt cannot be rejected" });

            return Ok(new { message = "Goods receipt rejected successfully" });
        }
    }
}
