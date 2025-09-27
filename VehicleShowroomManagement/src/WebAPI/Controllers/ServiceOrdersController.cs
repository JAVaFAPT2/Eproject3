using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.StartServiceOrder;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CompleteServiceOrder;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Queries.GetServiceOrderById;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Queries.GetServiceOrders;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for service order management
    /// </summary>
    [ApiController]
    [Route("api/service-orders")]
    [Authorize]
    public class ServiceOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiceOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all service orders with pagination and filters
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetServiceOrders(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] ServiceOrderStatus? status = null,
            [FromQuery] ServiceType? serviceType = null,
            [FromQuery] string? vehicleId = null,
            [FromQuery] string? customerId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetServiceOrdersQuery(
                pageNumber, pageSize, status, serviceType, vehicleId, customerId, fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets a service order by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetServiceOrder(string id)
        {
            var query = new GetServiceOrderByIdQuery(id);
            var serviceOrder = await _mediator.Send(query);

            if (serviceOrder == null)
                return NotFound(new { message = "Service order not found" });

            return Ok(serviceOrder);
        }

        /// <summary>
        /// Creates a new service order
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateServiceOrder([FromBody] CreateServiceOrderRequest request)
        {
            var command = new CreateServiceOrderCommand(
                request.ServiceOrderNumber,
                request.VehicleId,
                request.CustomerId,
                request.ServiceType,
                request.Description,
                request.ScheduledDate,
                request.EstimatedCost,
                request.AssignedTechnician);

            var serviceOrderId = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(GetServiceOrder), new { id = serviceOrderId }, 
                new { id = serviceOrderId, message = "Service order created successfully" });
        }

        /// <summary>
        /// Starts a service order
        /// </summary>
        [HttpPut("{id}/start")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> StartServiceOrder(string id, [FromBody] StartServiceOrderRequest request)
        {
            var command = new StartServiceOrderCommand(id, request.StartedBy, request.ActualStartDate, request.Notes);
            await _mediator.Send(command);

            return Ok(new { message = "Service order started successfully" });
        }

        /// <summary>
        /// Completes a service order
        /// </summary>
        [HttpPut("{id}/complete")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CompleteServiceOrder(string id, [FromBody] CompleteServiceOrderRequest request)
        {
            var command = new CompleteServiceOrderCommand(
                id, 
                request.CompletedBy, 
                request.CompletionDate, 
                request.ActualCost, 
                request.CompletionNotes);
            
            await _mediator.Send(command);

            return Ok(new { message = "Service order completed successfully" });
        }
    }

    /// <summary>
    /// Request model for creating service order
    /// </summary>
    public class CreateServiceOrderRequest
    {
        public string ServiceOrderNumber { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string? CustomerId { get; set; }
        public ServiceType ServiceType { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public decimal EstimatedCost { get; set; }
        public string? AssignedTechnician { get; set; }
    }

    /// <summary>
    /// Request model for starting service order
    /// </summary>
    public class StartServiceOrderRequest
    {
        public string StartedBy { get; set; } = string.Empty;
        public DateTime ActualStartDate { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Request model for completing service order
    /// </summary>
    public class CompleteServiceOrderRequest
    {
        public string CompletedBy { get; set; } = string.Empty;
        public DateTime CompletionDate { get; set; }
        public decimal ActualCost { get; set; }
        public string? CompletionNotes { get; set; }
    }
}