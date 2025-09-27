using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.VehicleRegistrations.Commands.CreateVehicleRegistration;
using VehicleShowroomManagement.Application.Features.VehicleRegistrations.Commands.UpdateRegistrationStatus;
using VehicleShowroomManagement.Application.Features.VehicleRegistrations.Queries.GetVehicleRegistrationById;
using VehicleShowroomManagement.Application.Features.VehicleRegistrations.Queries.GetVehicleRegistrations;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for vehicle registration management
    /// </summary>
    [ApiController]
    [Route("api/vehicle-registrations")]
    [Authorize]
    public class VehicleRegistrationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehicleRegistrationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all vehicle registrations with pagination and filters
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetVehicleRegistrations(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? status = null,
            [FromQuery] string? vehicleId = null,
            [FromQuery] string? customerId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetVehicleRegistrationsQuery(
                pageNumber, pageSize, status, vehicleId, customerId, fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets a vehicle registration by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetVehicleRegistration(string id)
        {
            var query = new GetVehicleRegistrationByIdQuery(id);
            var registration = await _mediator.Send(query);

            if (registration == null)
                return NotFound(new { message = "Vehicle registration not found" });

            return Ok(registration);
        }

        /// <summary>
        /// Creates a new vehicle registration
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateVehicleRegistration([FromBody] CreateVehicleRegistrationRequest request)
        {
            var command = new CreateVehicleRegistrationCommand(
                request.VehicleId,
                request.CustomerId,
                request.RegistrationNumber,
                request.RegistrationDate,
                request.ExpiryDate,
                request.RegistrationFee,
                request.ProcessedBy);

            var registrationId = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(GetVehicleRegistration), new { id = registrationId }, 
                new { id = registrationId, message = "Vehicle registration created successfully" });
        }

        /// <summary>
        /// Updates vehicle registration status
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateRegistrationStatus(string id, [FromBody] UpdateRegistrationStatusRequest request)
        {
            var command = new UpdateRegistrationStatusCommand(id, request.Status, request.Notes);
            await _mediator.Send(command);

            return Ok(new { message = "Registration status updated successfully" });
        }
    }

    /// <summary>
    /// Request model for creating vehicle registration
    /// </summary>
    public class CreateVehicleRegistrationRequest
    {
        public string VehicleId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal RegistrationFee { get; set; }
        public string ProcessedBy { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for updating registration status
    /// </summary>
    public class UpdateRegistrationStatusRequest
    {
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}