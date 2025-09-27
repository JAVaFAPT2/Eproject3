using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Queries.GetBillingDocumentById;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BillingDocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BillingDocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new billing document
        /// </summary>
        /// <param name="command">Billing document creation details</param>
        /// <returns>The ID of the created billing document</returns>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateBillingDocument([FromBody] CreateBillingDocumentCommand command)
        {
            try
            {
                var billingDocumentId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetBillingDocument), new { id = billingDocumentId }, new { Id = billingDocumentId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while creating the billing document", Details = ex.Message });
            }
        }

        /// <summary>
        /// Gets a billing document by ID
        /// </summary>
        /// <param name="id">Billing document ID</param>
        /// <returns>Billing document details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,HR,Admin")]
        public async Task<IActionResult> GetBillingDocument(string id)
        {
            try
            {
                var query = new GetBillingDocumentByIdQuery { BillingDocumentId = id };
                var billingDocument = await _mediator.Send(query);
                return Ok(billingDocument);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while retrieving the billing document", Details = ex.Message });
            }
        }
    }
}