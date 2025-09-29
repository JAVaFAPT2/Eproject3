using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.WaitingLists.Commands.AddToWaitingList;
using VehicleShowroomManagement.Application.Features.WaitingLists.Queries.GetWaitingList;
using VehicleShowroomManagement.WebAPI.Models.WaitingList;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for waiting list management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WaitingListController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Adds a customer to the waiting list for a specific vehicle model
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Customer,Dealer,Admin,HR")]
        public async Task<IActionResult> AddToWaitingList([FromBody] AddToWaitingListRequest request)
        {
            var command = new AddToWaitingListCommand(
                request.CustomerId,
                request.ModelNumber,
                request.PreferredDate,
                request.Notes);

            var waitingListId = await mediator.Send(command);

            return CreatedAtAction(nameof(GetWaitingList), new { id = waitingListId },
                new { id = waitingListId, message = "Customer added to waiting list successfully" });
        }

        /// <summary>
        /// Gets waiting list entries with pagination and filters
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetWaitingList(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? modelNumber = null,
            [FromQuery] string? customerId = null,
            [FromQuery] string? status = null)
        {
            var query = new GetWaitingListQuery(pageNumber, pageSize, modelNumber, customerId, status);
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}
