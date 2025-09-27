using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.ReturnRequests.Commands.CreateReturnRequest;
using VehicleShowroomManagement.Application.Features.ReturnRequests.Queries.GetReturnRequestById;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReturnRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReturnRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new return request
        /// </summary>
        /// <param name="command">Return request creation details</param>
        /// <returns>The ID of the created return request</returns>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateReturnRequest([FromBody] CreateReturnRequestCommand command)
        {
            try
            {
                var returnRequestId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetReturnRequest), new { id = returnRequestId }, new { Id = returnRequestId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while creating the return request", Details = ex.Message });
            }
        }

        /// <summary>
        /// Gets a return request by ID
        /// </summary>
        /// <param name="id">Return request ID</param>
        /// <returns>Return request details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,HR,Admin")]
        public async Task<IActionResult> GetReturnRequest(string id)
        {
            try
            {
                var query = new GetReturnRequestByIdQuery { ReturnRequestId = id };
                var returnRequest = await _mediator.Send(query);
                return Ok(returnRequest);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while retrieving the return request", Details = ex.Message });
            }
        }
    }
}