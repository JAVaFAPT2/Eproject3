using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.VehicleSpecs.Commands.AddVehicleSpec;
using VehicleShowroomManagement.Application.Features.VehicleSpecs.Commands.UpdateVehicleSpec;
using VehicleShowroomManagement.Application.Features.VehicleSpecs.Commands.DeleteVehicleSpec;
using VehicleShowroomManagement.Application.Features.VehicleSpecs.Queries.GetVehicleSpecs;
using VehicleShowroomManagement.Application.Features.VehicleSpecs.Queries.GetSpecById;
using VehicleShowroomManagement.WebAPI.Models.VehicleSpecs;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for vehicle specification management operations
    /// </summary>
    [ApiController]
    [Route("api/vehicle-models/{modelNumber}/specs")]
    [Authorize]
    public class VehicleSpecsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehicleSpecsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all specifications for a specific vehicle
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<VehicleSpecDto>>> GetVehicleSpecs(string modelNumber)
        {
            var query = new GetVehicleSpecsQuery(modelNumber);
            var specs = await _mediator.Send(query);
            return Ok(specs);
        }

        /// <summary>
        /// Gets a specific specification by ID
        /// </summary>
        [HttpGet("~/api/specs/{specId}")]
        public async Task<ActionResult<VehicleSpecDto>> GetSpec(string specId)
        {
            var query = new GetSpecByIdQuery(specId);
            var spec = await _mediator.Send(query);

            if (spec == null)
                return NotFound(new { message = "Specification not found" });

            return Ok(spec);
        }

        /// <summary>
        /// Adds a new specification to a vehicle
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> AddVehicleSpec(string modelNumber, [FromBody] AddVehicleSpecRequest request)
        {
            var command = new AddVehicleSpecCommand(
                modelNumber,
                request.SpecName,
                request.SpecValue,
                request.DisplayOrder,
                request.GroupName);

            var specId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetSpec), new { specId }, 
                new { id = specId, message = "Specification added successfully" });
        }

        /// <summary>
        /// Updates a vehicle specification
        /// </summary>
        [HttpPut("~/api/specs/{specId}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateVehicleSpec(string specId, [FromBody] UpdateVehicleSpecRequest request)
        {
            var command = new UpdateVehicleSpecCommand(
                specId,
                request.SpecValue,
                request.DisplayOrder,
                request.GroupName);

            await _mediator.Send(command);

            return Ok(new { message = "Specification updated successfully" });
        }

        /// <summary>
        /// Deletes a vehicle specification
        /// </summary>
        [HttpDelete("~/api/specs/{specId}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> DeleteVehicleSpec(string specId)
        {
            var command = new DeleteVehicleSpecCommand(specId);
            await _mediator.Send(command);

            return Ok(new { message = "Specification deleted successfully" });
        }
    }
}

