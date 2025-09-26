using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using VehicleShowroomManagement.Application.Queries;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Returns controller for return requests management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReturnsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReturnsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get return requests
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetReturns(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = new GetReturnsQuery(pageNumber, pageSize);
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}