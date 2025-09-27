using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.WaitingLists.Commands.CreateWaitingList;
using VehicleShowroomManagement.Application.Features.WaitingLists.Queries.GetWaitingListById;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WaitingListsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WaitingListsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new waiting list entry
        /// </summary>
        /// <param name="command">Waiting list creation details</param>
        /// <returns>The ID of the created waiting list entry</returns>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateWaitingList([FromBody] CreateWaitingListCommand command)
        {
            try
            {
                var waitingListId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetWaitingList), new { id = waitingListId }, new { Id = waitingListId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while creating the waiting list entry", Details = ex.Message });
            }
        }

        /// <summary>
        /// Gets a waiting list entry by ID
        /// </summary>
        /// <param name="id">Waiting list entry ID</param>
        /// <returns>Waiting list entry details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,HR,Admin")]
        public async Task<IActionResult> GetWaitingList(string id)
        {
            try
            {
                var query = new GetWaitingListByIdQuery { WaitingListId = id };
                var waitingList = await _mediator.Send(query);
                return Ok(waitingList);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while retrieving the waiting list entry", Details = ex.Message });
            }
        }
    }
}