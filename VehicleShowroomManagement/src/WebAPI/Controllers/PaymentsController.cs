using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Payments.Commands.CreatePayment;
using VehicleShowroomManagement.Application.Features.Payments.Queries.GetPaymentById;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new payment
        /// </summary>
        /// <param name="command">Payment creation details</param>
        /// <returns>The ID of the created payment</returns>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand command)
        {
            try
            {
                var paymentId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetPayment), new { id = paymentId }, new { Id = paymentId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while creating the payment", Details = ex.Message });
            }
        }

        /// <summary>
        /// Gets a payment by ID
        /// </summary>
        /// <param name="id">Payment ID</param>
        /// <returns>Payment details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,HR,Admin")]
        public async Task<IActionResult> GetPayment(string id)
        {
            try
            {
                var query = new GetPaymentByIdQuery { PaymentId = id };
                var payment = await _mediator.Send(query);
                return Ok(payment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while retrieving the payment", Details = ex.Message });
            }
        }
    }
}