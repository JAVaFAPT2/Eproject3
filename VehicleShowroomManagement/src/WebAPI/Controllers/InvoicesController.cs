using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Invoices.Commands.CreateInvoice;
using VehicleShowroomManagement.Application.Features.Invoices.Queries.GetInvoiceById;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new invoice
        /// </summary>
        /// <param name="command">Invoice creation details</param>
        /// <returns>The ID of the created invoice</returns>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceCommand command)
        {
            try
            {
                var invoiceId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetInvoice), new { id = invoiceId }, new { Id = invoiceId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while creating the invoice", Details = ex.Message });
            }
        }

        /// <summary>
        /// Gets an invoice by ID
        /// </summary>
        /// <param name="id">Invoice ID</param>
        /// <returns>Invoice details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,HR,Admin")]
        public async Task<IActionResult> GetInvoice(string id)
        {
            try
            {
                var query = new GetInvoiceByIdQuery { InvoiceId = id };
                var invoice = await _mediator.Send(query);
                return Ok(invoice);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while retrieving the invoice", Details = ex.Message });
            }
        }
    }
}