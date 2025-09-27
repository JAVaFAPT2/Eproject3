using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Billing.Commands.CreateInvoice;
using VehicleShowroomManagement.Application.Features.Billing.Commands.ProcessPayment;
using VehicleShowroomManagement.Application.Features.Billing.Commands.ApplyCredit;
using VehicleShowroomManagement.Application.Features.Billing.Queries.GetInvoiceById;
using VehicleShowroomManagement.Application.Features.Billing.Queries.GetInvoices;
using VehicleShowroomManagement.Application.Features.Billing.Queries.GetPaymentHistory;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for billing and invoicing operations
    /// </summary>
    [ApiController]
    [Route("api/billing")]
    [Authorize]
    public class BillingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BillingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all invoices with pagination and filters
        /// </summary>
        [HttpGet("invoices")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetInvoices(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? status = null,
            [FromQuery] string? customerId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetInvoicesQuery(pageNumber, pageSize, status, customerId, fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets an invoice by ID
        /// </summary>
        [HttpGet("invoices/{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetInvoice(string id)
        {
            var query = new GetInvoiceByIdQuery(id);
            var invoice = await _mediator.Send(query);

            if (invoice == null)
                return NotFound(new { message = "Invoice not found" });

            return Ok(invoice);
        }

        /// <summary>
        /// Creates a new invoice
        /// </summary>
        [HttpPost("invoices")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequest request)
        {
            var command = new CreateInvoiceCommand(
                request.OrderId,
                request.InvoiceNumber,
                request.InvoiceDate,
                request.DueDate,
                request.SubTotal,
                request.TaxAmount,
                request.TotalAmount,
                request.Notes);

            var invoiceId = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(GetInvoice), new { id = invoiceId }, 
                new { id = invoiceId, message = "Invoice created successfully" });
        }

        /// <summary>
        /// Processes payment for an invoice
        /// </summary>
        [HttpPost("invoices/{id}/payment")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ProcessPayment(string id, [FromBody] ProcessPaymentRequest request)
        {
            var command = new ProcessPaymentCommand(
                id,
                request.Amount,
                request.PaymentMethod,
                request.PaymentReference,
                request.ProcessedBy);

            await _mediator.Send(command);

            return Ok(new { message = "Payment processed successfully" });
        }

        /// <summary>
        /// Applies credit to customer account
        /// </summary>
        [HttpPost("credit/apply")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ApplyCredit([FromBody] ApplyCreditRequest request)
        {
            var command = new ApplyCreditCommand(
                request.CustomerId,
                request.Amount,
                request.Reason,
                request.AppliedBy);

            await _mediator.Send(command);

            return Ok(new { message = "Credit applied successfully" });
        }

        /// <summary>
        /// Gets payment history for a customer
        /// </summary>
        [HttpGet("payments/customer/{customerId}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetPaymentHistory(
            string customerId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetPaymentHistoryQuery(customerId, pageNumber, pageSize, fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }

    /// <summary>
    /// Request model for creating invoice
    /// </summary>
    public class CreateInvoiceRequest
    {
        public string OrderId { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Request model for processing payment
    /// </summary>
    public class ProcessPaymentRequest
    {
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? PaymentReference { get; set; }
        public string ProcessedBy { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for applying credit
    /// </summary>
    public class ApplyCreditRequest
    {
        public string CustomerId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string AppliedBy { get; set; } = string.Empty;
    }
}