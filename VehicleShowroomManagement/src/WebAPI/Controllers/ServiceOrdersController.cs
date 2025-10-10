using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder;
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
}
