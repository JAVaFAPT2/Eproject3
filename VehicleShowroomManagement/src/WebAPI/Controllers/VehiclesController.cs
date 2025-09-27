using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Vehicles.Commands.CreateVehicle;
using VehicleShowroomManagement.Application.Features.Vehicles.Commands.UpdateVehicleStatus;
using VehicleShowroomManagement.Application.Features.Vehicles.Queries.GetVehicleById;
using VehicleShowroomManagement.Application.Features.Vehicles.Queries.SearchVehicles;
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
        public async Task<ActionResult<VehicleDto>> GetVehicle(string id)
        {
            var query = new GetVehicleByIdQuery(id);
            var vehicle = await _mediator.Send(query);

            if (vehicle == null)
                return NotFound(new { message = "Vehicle not found" });

            return Ok(vehicle);
        }

        /// <summary>
        /// Searches vehicles with filters - critical for showroom operations
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<SearchVehiclesResult>> SearchVehicles(
            [FromQuery] string? searchTerm = null,
            [FromQuery] VehicleStatus? status = null,
            [FromQuery] string? modelNumber = null,
            [FromQuery] string? brand = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new SearchVehiclesQuery(
                searchTerm,
                status,
                modelNumber,
                brand,
                minPrice,
                maxPrice,
                pageNumber,
                pageSize);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Updates vehicle status - for inventory management
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateVehicleStatus(string id, [FromBody] UpdateVehicleStatusRequest request)
        {
            var command = new UpdateVehicleStatusCommand(id, request.Status);
            await _mediator.Send(command);
            
            return Ok(new { message = "Vehicle status updated successfully" });
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

    /// <summary>
    /// Request model for updating vehicle status
    /// </summary>
    public class UpdateVehicleStatusRequest
    {
        public VehicleStatus Status { get; set; }
    }
}