using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Features.VehicleModels.Commands.CreateVehicleModel;
using VehicleShowroomManagement.Application.Features.VehicleModels.Commands.UpdateVehicleModel;
using VehicleShowroomManagement.Application.Features.VehicleModels.Commands.DeleteVehicleModel;
using VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModels;
using VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModelById;
using VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModelBySlug;
using VehicleShowroomManagement.Application.Features.VehicleModels.Queries.SearchLevel2Models;
// using VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.AddVehiclePhoto;
using VehicleShowroomManagement.WebAPI.Models.VehicleModels;
namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehicleModelsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateVehicleModel([FromBody] CreateVehicleModelRequest request)
        {
            if (request == null)
                return BadRequest(new { message = "Invalid request body" });

            // Generate ModelNumber if not provided
            var modelNumber = !string.IsNullOrWhiteSpace(request.ModelNumber)
                ? request.ModelNumber
                : $"MODEL-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

            var command = new CreateVehicleModelCommand(
                modelNumber,
                request.Name,
                request.Price,
                request.Description,
                request.ParentId,
                request.Level,
                request.Slug);

            var createdModelNumber = await mediator.Send(command);
            return Ok(new { modelNumber = createdModelNumber, message = "Vehicle model created successfully" });
        }

        /// <summary>
        /// Update vehicle model
        /// </summary>
        [HttpPut("{modelNumber}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateVehicleModel(string modelNumber, [FromBody] UpdateVehicleModelRequest request)
        {
            var command = new UpdateVehicleModelCommand(
                modelNumber,
                request.Name,
                request.Price,
                request.Description,
                request.ParentId,
                request.Level,
                request.Slug);

            await mediator.Send(command);
            return Ok(new { message = "Vehicle model updated successfully" });
        }

        /// <summary>
        /// Gets a vehicle model by model number
        /// </summary>
        [HttpGet("{modelNumber}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVehicleModel(string modelNumber)
        {
            var query = new GetVehicleModelByIdQuery(modelNumber);
            var result = await mediator.Send(query);
            
            if (result == null)
                return NotFound(new { message = "Vehicle model not found" });
                
            return Ok(result);
        }

        /// <summary>
        /// Gets a level-2 vehicle model by slug
        /// </summary>
        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVehicleModelBySlug(string slug)
        {
            var result = await mediator.Send(new GetVehicleModelBySlugQuery(slug));
            if (result == null) return NotFound(new { message = "Vehicle model not found" });
            return Ok(result);
        }

        /// <summary>
        /// Gets all vehicle models with pagination
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetVehicleModels(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var query = new GetVehicleModelsQuery(pageNumber, pageSize, search);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Search level-2 vehicle models by parent and specs
        /// </summary>
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchLevel2Models(
            [FromQuery] string? parentModelNumber = null,
            [FromQuery] int? seats = null,
            [FromQuery] string? fuelType = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new SearchLevel2ModelsQuery(parentModelNumber, seats, fuelType, pageNumber, pageSize);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Soft delete a vehicle model by model number
        /// </summary>
        [HttpDelete("{modelNumber}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> DeleteVehicleModel(string modelNumber)
        {
            var command = new DeleteVehicleModelCommand(modelNumber);
            await mediator.Send(command);
            return Ok(new { message = "Vehicle model deleted successfully" });
        }
    }
}

