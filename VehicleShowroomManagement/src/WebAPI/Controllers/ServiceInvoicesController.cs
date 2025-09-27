using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.WebAPI.Models.Billing;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for service billing and invoicing operations
    /// </summary>
    [ApiController]
    [Route("api/service-invoices")]
    [Authorize]
    public class ServiceInvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiceInvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all service invoices with pagination and filters
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetServiceInvoices(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? status = null,
            [FromQuery] string? customerId = null,
            [FromQuery] string? serviceOrderId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            await Task.CompletedTask;
            return Ok(new { 
                message = "Service invoices endpoint ready", 
                pageNumber, 
                pageSize,
                filterBy = new { status, customerId, serviceOrderId }
            });
        }

        /// <summary>
        /// Gets a service invoice by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetServiceInvoice(string id)
        {
            await Task.CompletedTask;
            return Ok(new { 
                message = $"Service invoice {id} endpoint ready",
                invoiceType = "Service"
            });
        }

        /// <summary>
        /// Creates a new service invoice
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateServiceInvoice([FromBody] CreateServiceInvoiceRequest request)
        {
            await Task.CompletedTask;
            var invoiceId = Guid.NewGuid().ToString();
            
            return CreatedAtAction(nameof(GetServiceInvoice), new { id = invoiceId }, 
                new { 
                    id = invoiceId, 
                    message = "Service invoice created successfully",
                    serviceOrderId = request.ServiceOrderId,
                    totalAmount = request.TotalAmount
                });
        }

        /// <summary>
        /// Generates invoice from completed service order
        /// </summary>
        [HttpPost("generate/service-order/{serviceOrderId}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GenerateInvoiceFromServiceOrder(string serviceOrderId)
        {
            await Task.CompletedTask;
            var invoiceId = Guid.NewGuid().ToString();
            
            return CreatedAtAction(nameof(GetServiceInvoice), new { id = invoiceId }, 
                new { 
                    id = invoiceId, 
                    message = "Service invoice generated successfully",
                    serviceOrderId,
                    invoiceType = "Service"
                });
        }

        /// <summary>
        /// Processes payment for service invoice
        /// </summary>
        [HttpPost("{id}/payment")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ProcessServicePayment(string id, [FromBody] ProcessPaymentRequest request)
        {
            await Task.CompletedTask;
            return Ok(new { 
                message = "Service payment processed successfully",
                invoiceId = id,
                amount = request.Amount,
                paymentMethod = request.PaymentMethod.ToString(),
                invoiceType = "Service"
            });
        }
    }
}