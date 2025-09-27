using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Vehicles.Commands.CreateVehicle;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for vehicle management operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehiclesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehiclesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new vehicle
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
        {
            var command = new CreateVehicleCommand(
                request.VehicleId,
                request.ModelNumber,
                request.PurchasePrice,
                request.ExternalNumber,
                request.Vin,
                request.LicensePlate,
                request.ReceiptDate);

            var vehicleId = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(GetVehicle), new { id = vehicleId }, 
                new { id = vehicleId, message = "Vehicle created successfully" });
        }

        /// <summary>
        /// Gets a vehicle by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(string id)
        {
            // TODO: Implement GetVehicleByIdQuery
            return Ok(new { message = "Vehicle retrieval not implemented yet" });
        }
    }

    /// <summary>
    /// Request model for creating a vehicle
    /// </summary>
    public class CreateVehicleRequest
    {
        public string VehicleId { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public string? ExternalNumber { get; set; }
        public string? Vin { get; set; }
        public string? LicensePlate { get; set; }
        public DateTime? ReceiptDate { get; set; }
    }
}