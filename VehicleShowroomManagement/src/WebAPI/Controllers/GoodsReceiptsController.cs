using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.GoodsReceipts.Commands.CreateGoodsReceipt;
using VehicleShowroomManagement.Application.Features.GoodsReceipts.Commands.AcceptGoodsReceipt;
using VehicleShowroomManagement.Application.Features.GoodsReceipts.Queries.GetGoodsReceiptById;
using VehicleShowroomManagement.Application.Features.GoodsReceipts.Queries.GetGoodsReceipts;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for goods receipt management
    /// </summary>
    [ApiController]
    [Route("api/goods-receipts")]
    [Authorize]
    public class GoodsReceiptsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GoodsReceiptsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all goods receipts with pagination and filters
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetGoodsReceipts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] GoodsReceiptStatus? status = null,
            [FromQuery] string? purchaseOrderId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetGoodsReceiptsQuery(pageNumber, pageSize, status, purchaseOrderId, fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets a goods receipt by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetGoodsReceipt(string id)
        {
            var query = new GetGoodsReceiptByIdQuery(id);
            var receipt = await _mediator.Send(query);

            if (receipt == null)
                return NotFound(new { message = "Goods receipt not found" });

            return Ok(receipt);
        }

        /// <summary>
        /// Creates a new goods receipt
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateGoodsReceipt([FromBody] CreateGoodsReceiptRequest request)
        {
            var command = new CreateGoodsReceiptCommand(
                request.ReceiptNumber,
                request.PurchaseOrderId,
                request.ReceivedDate,
                request.ReceivedBy,
                request.VehicleDetails);

            var receiptId = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(GetGoodsReceipt), new { id = receiptId }, 
                new { id = receiptId, message = "Goods receipt created successfully" });
        }

        /// <summary>
        /// Accepts a goods receipt after inspection
        /// </summary>
        [HttpPut("{id}/accept")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> AcceptGoodsReceipt(string id, [FromBody] AcceptGoodsReceiptRequest request)
        {
            var command = new AcceptGoodsReceiptCommand(
                id, 
                request.InspectedBy, 
                request.InspectionNotes,
                request.AcceptedVehicles);
            
            await _mediator.Send(command);

            return Ok(new { message = "Goods receipt accepted successfully" });
        }
    }

    /// <summary>
    /// Request model for creating a goods receipt
    /// </summary>
    public class CreateGoodsReceiptRequest
    {
        public string ReceiptNumber { get; set; } = string.Empty;
        public string PurchaseOrderId { get; set; } = string.Empty;
        public DateTime ReceivedDate { get; set; }
        public string ReceivedBy { get; set; } = string.Empty;
        public List<ReceivedVehicleRequest> VehicleDetails { get; set; } = new List<ReceivedVehicleRequest>();
    }

    /// <summary>
    /// Request model for received vehicle details
    /// </summary>
    public class ReceivedVehicleRequest
    {
        public string VehicleId { get; set; } = string.Empty;
        public string Vin { get; set; } = string.Empty;
        public string EngineNumber { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string? Condition { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Request model for accepting goods receipt
    /// </summary>
    public class AcceptGoodsReceiptRequest
    {
        public string InspectedBy { get; set; } = string.Empty;
        public string? InspectionNotes { get; set; }
        public List<string> AcceptedVehicles { get; set; } = new List<string>();
    }
}