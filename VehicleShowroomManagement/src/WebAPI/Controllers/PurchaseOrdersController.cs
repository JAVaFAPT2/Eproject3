using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder;
using VehicleShowroomManagement.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PurchaseOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new purchase order
        /// </summary>
        /// <param name="command">Purchase order creation details</param>
        /// <returns>The ID of the created purchase order</returns>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderCommand command)
        {
            try
            {
                var purchaseOrderId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetPurchaseOrder), new { id = purchaseOrderId }, new { Id = purchaseOrderId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while creating the purchase order", Details = ex.Message });
            }
        }

        /// <summary>
        /// Gets a purchase order by ID
        /// </summary>
        /// <param name="id">Purchase order ID</param>
        /// <returns>Purchase order details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,HR,Admin")]
        public async Task<IActionResult> GetPurchaseOrder(string id)
        {
            try
            {
                var query = new GetPurchaseOrderByIdQuery { PurchaseOrderId = id };
                var purchaseOrder = await _mediator.Send(query);
                return Ok(purchaseOrder);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while retrieving the purchase order", Details = ex.Message });
            }
        }
    }
}