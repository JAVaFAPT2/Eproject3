using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Application.Commands;
using VehicleShowroomManagement.Application.Queries;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Vehicles controller for vehicle inventory management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehiclesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehiclesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all vehicles with optional filtering and search
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetVehicles(
            [FromQuery] string? searchTerm,
            [FromQuery] string? status,
            [FromQuery] string? brand,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = new GetVehiclesQuery(searchTerm, status, brand, pageNumber, pageSize);
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get vehicle by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(string id)
        {
            try
            {
                var query = new GetVehicleByIdQuery(id);
                var result = await _mediator.Send(query);

                if (result == null)
                {
                    return NotFound(new { message = "Vehicle not found" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create new vehicle
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
        {
            try
            {
                var command = new CreateVehicleCommand(
                    request.ModelNumber,
                    request.Name,
                    request.Brand,
                    request.Price,
                    request.Status,
                    request.RegistrationNumber,
                    request.RegistrationDate,
                    request.ExternalId);

                var result = await _mediator.Send(command);

                return CreatedAtAction(nameof(GetVehicle), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update vehicle
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateVehicle(string id, [FromBody] UpdateVehicleRequest request)
        {
            try
            {
                var command = new UpdateVehicleCommand(
                    id,
                    request.Price,
                    request.Status);

                await _mediator.Send(command);

                return Ok(new { message = "Vehicle updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Soft delete vehicle (set deleted_at)
        /// </summary>
        [HttpPost("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> DeleteVehicle(string id)
        {
            try
            {
                var command = new DeleteVehicleCommand(id);
                await _mediator.Send(command);

                return Ok(new { message = "Vehicle deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Soft delete multiple vehicles (set deleted_at)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> DeleteVehicles([FromBody] DeleteVehiclesRequest request)
        {
            try
            {
                var command = new DeleteVehiclesCommand(request.Ids);
                await _mediator.Send(command);

                return Ok(new { message = "Vehicles deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    /// <summary>
    /// Request model for creating a vehicle
    /// </summary>
    public class CreateVehicleRequest
    {
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = "AVAILABLE";
        public string RegistrationNumber { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public string ExternalId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for updating a vehicle
    /// </summary>
    public class UpdateVehicleRequest
    {
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for deleting multiple vehicles
    /// </summary>
    public class DeleteVehiclesRequest
    {
        public List<string> Ids { get; set; } = new List<string>();
    }
}