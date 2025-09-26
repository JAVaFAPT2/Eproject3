using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using VehicleShowroomManagement.Application.Queries;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Customers controller for customer management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Get all customers
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCustomers(
            [FromQuery] string? searchTerm,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = new GetCustomersQuery(searchTerm, pageNumber, pageSize);
                var result = await mediator.Send(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get orders by customer
        /// </summary>
        [HttpGet("{customerId}/orders")]
        public async Task<IActionResult> GetCustomerOrders(string customerId)
        {
            try
            {
                var query = new GetCustomerOrdersQuery(customerId);
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