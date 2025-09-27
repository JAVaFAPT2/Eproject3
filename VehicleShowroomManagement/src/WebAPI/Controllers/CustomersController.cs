using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Customers.Commands.CreateCustomer;
using VehicleShowroomManagement.WebAPI.Models.Customers;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for customer management operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            var command = new CreateCustomerCommand(
                request.CustomerId,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Phone,
                request.Street,
                request.City,
                request.State,
                request.ZipCode,
                request.Cccd);

            var customerId = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(GetCustomer), new { id = customerId }, 
                new { id = customerId, message = "Customer created successfully" });
        }

        /// <summary>
        /// Gets a customer by ID
        /// </summary>
        [HttpGet("{id}")]
        public Task<IActionResult> GetCustomer(string id)
        {
            // TODO: Implement GetCustomerByIdQuery when needed
            return Task.FromResult<IActionResult>(Ok(new { message = "Customer retrieval not implemented yet" }));
        }
    }
}