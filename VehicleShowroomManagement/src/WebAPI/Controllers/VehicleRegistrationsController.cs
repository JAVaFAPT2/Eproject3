using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.VehicleRegistrations.Commands.CreateVehicleRegistration;
using VehicleShowroomManagement.Application.Features.VehicleRegistrations.Queries.GetVehicleRegistrationById;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
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
        /// Creates a new vehicle registration
        /// </summary>
        /// <param name="command">Vehicle registration creation details</param>
        /// <returns>The ID of the created vehicle registration</returns>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateVehicleRegistration([FromBody] CreateVehicleRegistrationCommand command)
        {
            try
            {
                var vehicleRegistrationId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetVehicleRegistration), new { id = vehicleRegistrationId }, new { Id = vehicleRegistrationId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while creating the vehicle registration", Details = ex.Message });
            }
        }

        /// <summary>
        /// Gets a vehicle registration by ID
        /// </summary>
        /// <param name="id">Vehicle registration ID</param>
        /// <returns>Vehicle registration details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,HR,Admin")]
        public async Task<IActionResult> GetVehicleRegistration(string id)
        {
            try
            {
                var query = new GetVehicleRegistrationByIdQuery { VehicleRegistrationId = id };
                var vehicleRegistration = await _mediator.Send(query);
                return Ok(vehicleRegistration);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while retrieving the vehicle registration", Details = ex.Message });
            }
        }
    }
}