using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.UpdateAmount;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.UpdateAppointmentDate;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.UpdateStatus;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Queries.GetBillingDocuments;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Dealer,Admin")]
    public class BillingDocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BillingDocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all billing documents with pagination
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBillingDocuments(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? status = null,
            [FromQuery] string? orderId = null)
        {
            var query = new GetBillingDocumentsQuery(pageNumber, pageSize, status, orderId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new billing document
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateBillingDocument([FromBody] CreateBillingDocumentRequest request)
        {
            var command = new CreateBillingDocumentCommand(
                request.OrderId,
                request.CreatedBy,
                request.Amount,
                request.AppointmentDate);

            var billingDocId = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateBillingDocument), new { id = billingDocId }, 
                new { id = billingDocId, message = "Billing document created successfully" });
        }

        /// <summary>
        /// Updates the amount of a billing document
        /// </summary>
        [HttpPatch("{id}/amount")]
        public async Task<IActionResult> UpdateAmount(string id, [FromBody] UpdateAmountRequest request)
        {
            var command = new UpdateBillingDocumentAmountCommand(id, request.Amount);
            await _mediator.Send(command);
            return Ok(new { message = "Billing document amount updated successfully" });
        }

        /// <summary>
        /// Updates the appointment date of a billing document
        /// </summary>
        [HttpPatch("{id}/appointment-date")]
        public async Task<IActionResult> UpdateAppointmentDate(string id, [FromBody] UpdateAppointmentDateRequest request)
        {
            var command = new UpdateBillingDocumentAppointmentDateCommand(id, request.AppointmentDate);
            await _mediator.Send(command);
            return Ok(new { message = "Billing document appointment date updated successfully" });
        }

        /// <summary>
        /// Updates the status of a billing document (Paid, PartiallyPaid, Unpaid)
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateStatusRequest request)
        {
            var command = new UpdateBillingDocumentStatusCommand(id, request.Status);
            await _mediator.Send(command);
            return Ok(new { message = "Billing document status updated successfully" });
        }
    }

    public class CreateBillingDocumentRequest
    {
        public string OrderId { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }

    public class UpdateAmountRequest
    {
        public decimal Amount { get; set; }
    }

    public class UpdateAppointmentDateRequest
    {
        public DateTime? AppointmentDate { get; set; }
    }

    public class UpdateStatusRequest
    {
        public BillingStatus Status { get; set; }
    }
}

