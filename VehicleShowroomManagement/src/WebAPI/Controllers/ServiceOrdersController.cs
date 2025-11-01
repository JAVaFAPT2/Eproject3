using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.UpdateStatus;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Queries.GetServiceOrders;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Dealer,Admin,Customer")]
    public class ServiceOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiceOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all service orders with pagination
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetServiceOrders(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? status = null,
            [FromQuery] string? orderId = null,
            [FromQuery] string? customerId = null)
        {
            ServiceOrderStatus? statusEnum = null;
            if (status.HasValue)
            {
                // Map to enum: 1=Scheduled, 2=InProgress, 3=Completed, 4=Cancelled
                statusEnum = status.Value switch
                {
                    1 => ServiceOrderStatus.Scheduled,
                    2 => ServiceOrderStatus.InProgress,
                    3 => ServiceOrderStatus.Completed,
                    4 => ServiceOrderStatus.Cancelled,
                    _ => null
                };
            }

            var query = new GetServiceOrdersQuery(pageNumber, pageSize, statusEnum?.ToString(), orderId, customerId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new service order
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateServiceOrder([FromBody] CreateServiceOrderRequest request)
        {
            var command = new CreateServiceOrderCommand(
                request.OrderId,
                request.CustomerId,
                request.CreatedBy,
                request.Type,
                request.Cost,
                request.AppointmentDate,
                request.Description);

            var serviceOrderId = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateServiceOrder), new { id = serviceOrderId }, 
                new { id = serviceOrderId, message = "Service order created successfully" });
        }

        /// <summary>
        /// Update service order status
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateServiceOrderStatus(string id, [FromBody] UpdateServiceOrderStatusRequest request)
        {
            var command = new UpdateServiceOrderStatusCommand(id, request.Status);
            var result = await _mediator.Send(command);

            // Include billing document ID if created
            if (!string.IsNullOrEmpty(result.BillingDocumentId))
            {
                return Ok(new 
                { 
                    message = result.Message,
                    billingDocumentId = result.BillingDocumentId
                });
            }

            return Ok(new 
            { 
                message = result.Message
            });
        }
    }

    public class CreateServiceOrderRequest
    {
        public string OrderId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public ServiceType Type { get; set; }
        public decimal Cost { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateServiceOrderStatusRequest
    {
        public ServiceOrderStatus Status { get; set; }
    }
}
