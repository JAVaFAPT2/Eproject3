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
    [Authorize(Roles = "Dealer,Admin")]
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
            [FromQuery] string? status = null,
            [FromQuery] string? orderId = null)
        {
            var query = new GetServiceOrdersQuery(pageNumber, pageSize, status, orderId);
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
            var command = new UpdateServiceOrderStatusCommand(id, request.Status, request.LicensePlate);
            var result = await _mediator.Send(command);

            return Ok(new 
            { 
                message = result.Message
            });
        }
    }

    public class CreateServiceOrderRequest
    {
        public string OrderId { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public ServiceType Type { get; set; }
        public decimal Cost { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateServiceOrderStatusRequest
    {
        public ServiceOrderStatus Status { get; set; }
        public string? LicensePlate { get; set; }
    }
}
