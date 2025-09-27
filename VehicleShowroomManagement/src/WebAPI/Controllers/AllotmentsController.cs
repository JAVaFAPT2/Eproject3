using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Allotments.Commands.CreateAllotment;
using VehicleShowroomManagement.Application.Features.Allotments.Queries.GetAllotmentById;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AllotmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AllotmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new allotment
        /// </summary>
        /// <param name="command">Allotment creation details</param>
        /// <returns>The ID of the created allotment</returns>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateAllotment([FromBody] CreateAllotmentCommand command)
        {
            try
            {
                var allotmentId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetAllotment), new { id = allotmentId }, new { Id = allotmentId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while creating the allotment", Details = ex.Message });
            }
        }

        /// <summary>
        /// Gets an allotment by ID
        /// </summary>
        /// <param name="id">Allotment ID</param>
        /// <returns>Allotment details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,HR,Admin")]
        public async Task<IActionResult> GetAllotment(string id)
        {
            try
            {
                var query = new GetAllotmentByIdQuery { AllotmentId = id };
                var allotment = await _mediator.Send(query);
                return Ok(allotment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while retrieving the allotment", Details = ex.Message });
            }
        }
    }
}