using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument;

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
    }

    public class CreateBillingDocumentRequest
    {
        public string OrderId { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }
}

