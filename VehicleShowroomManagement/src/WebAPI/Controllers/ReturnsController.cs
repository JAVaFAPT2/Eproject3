using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Returns.Commands.CreateReturnRequest;
using VehicleShowroomManagement.Application.Features.Returns.Commands.ApproveReturnRequest;
using VehicleShowroomManagement.Application.Features.Returns.Commands.RejectReturnRequest;
using VehicleShowroomManagement.Application.Features.Returns.Queries.GetReturnRequestById;
using VehicleShowroomManagement.Application.Features.Returns.Queries.GetReturnRequests;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for returns management
    /// </summary>
    [ApiController]
    [Route("api/returns")]
    [Authorize]
    public class ReturnsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReturnsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all return requests with pagination and filters
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetReturnRequests(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? status = null,
            [FromQuery] string? customerId = null,
            [FromQuery] string? vehicleId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetReturnRequestsQuery(
                pageNumber, pageSize, status, customerId, vehicleId, fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets a return request by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetReturnRequest(string id)
        {
            var query = new GetReturnRequestByIdQuery(id);
            var returnRequest = await _mediator.Send(query);

            if (returnRequest == null)
                return NotFound(new { message = "Return request not found" });

            return Ok(returnRequest);
        }

        /// <summary>
        /// Creates a new return request
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateReturnRequest([FromBody] CreateReturnRequestRequest request)
        {
            var command = new CreateReturnRequestCommand(
                request.OrderId,
                request.VehicleId,
                request.CustomerId,
                request.Reason,
                request.Description,
                request.RequestedBy);

            var returnRequestId = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(GetReturnRequest), new { id = returnRequestId }, 
                new { id = returnRequestId, message = "Return request created successfully" });
        }

        /// <summary>
        /// Approves a return request
        /// </summary>
        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveReturnRequest(string id, [FromBody] ApproveReturnRequestRequest request)
        {
            var command = new ApproveReturnRequestCommand(
                id, 
                request.ApprovedBy, 
                request.RefundAmount, 
                request.ApprovalNotes);
            
            await _mediator.Send(command);

            return Ok(new { message = "Return request approved successfully" });
        }

        /// <summary>
        /// Rejects a return request
        /// </summary>
        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectReturnRequest(string id, [FromBody] RejectReturnRequestRequest request)
        {
            var command = new RejectReturnRequestCommand(id, request.RejectedBy, request.RejectionReason);
            await _mediator.Send(command);

            return Ok(new { message = "Return request rejected successfully" });
        }
    }

    /// <summary>
    /// Request model for creating return request
    /// </summary>
    public class CreateReturnRequestRequest
    {
        public string OrderId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string RequestedBy { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for approving return request
    /// </summary>
    public class ApproveReturnRequestRequest
    {
        public string ApprovedBy { get; set; } = string.Empty;
        public decimal RefundAmount { get; set; }
        public string? ApprovalNotes { get; set; }
    }

    /// <summary>
    /// Request model for rejecting return request
    /// </summary>
    public class RejectReturnRequestRequest
    {
        public string RejectedBy { get; set; } = string.Empty;
        public string RejectionReason { get; set; } = string.Empty;
    }
}