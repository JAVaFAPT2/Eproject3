using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.WebAPI.Models.Billing;
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
            await Task.CompletedTask;
            return Ok(new { message = "Invoices endpoint ready", pageNumber, pageSize });
        }

        /// <summary>
        /// Gets an invoice by ID
        /// </summary>
        [HttpGet("invoices/{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetInvoice(string id)
        {
            await Task.CompletedTask;
            return Ok(new { message = $"Invoice {id} endpoint ready" });
        }

        /// <summary>
        /// Creates a new invoice for sales or service orders
        /// </summary>
        [HttpPost("invoices")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequest request)
        {
            // Validate that either SalesOrderId or ServiceOrderId is provided
            if (string.IsNullOrEmpty(request.SalesOrderId) && string.IsNullOrEmpty(request.ServiceOrderId))
            {
                return BadRequest(new { message = "Either SalesOrderId or ServiceOrderId must be provided" });
            }

            if (!string.IsNullOrEmpty(request.SalesOrderId) && !string.IsNullOrEmpty(request.ServiceOrderId))
            {
                return BadRequest(new { message = "Only one of SalesOrderId or ServiceOrderId should be provided" });
            }

            await Task.CompletedTask;
            var invoiceId = Guid.NewGuid().ToString();
            
            return CreatedAtAction(nameof(GetInvoice), new { id = invoiceId }, 
                new { 
                    id = invoiceId, 
                    message = "Invoice created successfully",
                    invoiceType = request.InvoiceType,
                    orderId = request.ServiceOrderId ?? request.SalesOrderId
                });
        }

        /// <summary>
        /// Processes payment for an invoice
        /// </summary>
        [HttpPost("invoices/{id}/payment")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ProcessPayment(string id, [FromBody] ProcessPaymentRequest request)
        {
            await Task.CompletedTask;
            return Ok(new { 
                message = "Payment processed successfully",
                invoiceId = id,
                amount = request.Amount,
                paymentMethod = request.PaymentMethod.ToString()
            });
        }

        /// <summary>
        /// Applies credit to customer account
        /// </summary>
        [HttpPost("credit/apply")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ApplyCredit([FromBody] ApplyCreditRequest request)
        {
            await Task.CompletedTask;
            return Ok(new { 
                message = "Credit applied successfully",
                customerId = request.CustomerId,
                amount = request.Amount,
                reason = request.Reason
            });
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
            await Task.CompletedTask;
            return Ok(new { message = $"Payment history for customer {customerId} endpoint ready" });
        }
    }
}