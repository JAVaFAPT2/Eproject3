using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.VehicleRegistrations.Commands;
using VehicleShowroomManagement.Application.VehicleRegistrations.Queries;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing vehicle registrations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehicleRegistrationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehicleRegistrationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all vehicle registrations with filtering and pagination
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetVehicleRegistrations(
            [FromQuery] string? searchTerm,
            [FromQuery] string? status,
            [FromQuery] string? registrationState,
            [FromQuery] string? vehicleType,
            [FromQuery] string? fuelType,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetVehicleRegistrationsQuery
            {
                SearchTerm = searchTerm,
                Status = status,
                RegistrationState = registrationState,
                VehicleType = vehicleType,
                FuelType = fuelType,
                FromDate = fromDate,
                ToDate = toDate,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get vehicle registration by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetVehicleRegistration(string id)
        {
            var query = new GetVehicleRegistrationByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Get vehicle registration by VIN
        /// </summary>
        [HttpGet("vin/{vin}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetVehicleRegistrationByVin(string vin)
        {
            var query = new GetVehicleRegistrationByVinQuery { VIN = vin };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Create new vehicle registration
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateVehicleRegistration([FromBody] CreateVehicleRegistrationCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetVehicleRegistration), new { id = result }, result);
        }

        /// <summary>
        /// Update vehicle registration
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateVehicleRegistration(string id, [FromBody] UpdateVehicleRegistrationCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return Ok(new { message = "Vehicle registration updated successfully" });
        }

        /// <summary>
        /// Transfer vehicle ownership
        /// </summary>
        [HttpPost("{id}/transfer-ownership")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> TransferOwnership(string id, [FromBody] TransferOwnershipCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest(new { message = "Vehicle ownership cannot be transferred" });

            return Ok(new { message = "Vehicle ownership transferred successfully" });
        }
    }
}
