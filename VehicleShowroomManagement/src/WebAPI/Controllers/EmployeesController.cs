using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using VehicleShowroomManagement.Application.Queries;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Employees controller for employee management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Get all employees with search functionality
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> GetEmployees(
            [FromQuery] string? searchTerm,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = new GetEmployeesQuery(searchTerm, pageNumber, pageSize);
                var result = await mediator.Send(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}