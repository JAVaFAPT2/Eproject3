using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.VehicleImages.Commands.CreateVehicleImage;
using VehicleShowroomManagement.Application.Features.VehicleImages.Queries.GetVehicleImageById;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehicleImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehicleImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new vehicle image
        /// </summary>
        /// <param name="command">Vehicle image creation details</param>
        /// <returns>The ID of the created vehicle image</returns>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateVehicleImage([FromBody] CreateVehicleImageCommand command)
        {
            try
            {
                var vehicleImageId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetVehicleImage), new { id = vehicleImageId }, new { Id = vehicleImageId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while creating the vehicle image", Details = ex.Message });
            }
        }

        /// <summary>
        /// Gets a vehicle image by ID
        /// </summary>
        /// <param name="id">Vehicle image ID</param>
        /// <returns>Vehicle image details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,HR,Admin")]
        public async Task<IActionResult> GetVehicleImage(string id)
        {
            try
            {
                var query = new GetVehicleImageByIdQuery { VehicleImageId = id };
                var vehicleImage = await _mediator.Send(query);
                return Ok(vehicleImage);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while retrieving the vehicle image", Details = ex.Message });
            }
        }
    }
}