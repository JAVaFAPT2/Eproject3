using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Features.VehicleModels.Commands.CreateVehicleModel;
using VehicleShowroomManagement.Application.Features.VehicleModels.Commands.UpdateVehicleModel;
using VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModels;
using VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModelById;
using VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModelBySlug;
using VehicleShowroomManagement.Application.Features.VehicleModels.Queries.SearchLevel2Models;
using VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.AddVehiclePhoto;
using VehicleShowroomManagement.WebAPI.Models.VehicleModels;
namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehicleModelsController(IMediator mediator, ICloudinaryService cloudinaryService) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateVehicleModel([FromForm] string data, [FromForm] List<IFormFile>? files)
        {
            var request = System.Text.Json.JsonSerializer.Deserialize<CreateVehicleModelRequest>(data, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (request is null) return BadRequest(new { message = "Invalid 'data' JSON" });

            var command = new CreateVehicleModelCommand(
                request.ModelNumber,
                request.Name,
                request.Price,
                request.Description,
                request.ParentId,
                request.Level,
                request.Slug);

            var modelNumber = await mediator.Send(command);

            // Optional: upload photos for this modelNumber
            if (files is not null && files.Count > 0)
            {
                var order = 0;
                foreach (var f in files)
                {
                    if (f is not { Length: not 0 }) continue;
                    var upload = await cloudinaryService.UploadImageAsync(f, "vehicle-models");
                    if (order == 0)
                    {
                        // set primary photo on model (first image)
                        await mediator.Send(new UpdateVehicleModelCommand(modelNumber, request.Name, request.Price, request.Description, request.ParentId, request.Level, request.Slug, upload.SecureUrl));
                    }
                    await mediator.Send(new AddVehiclePhotoCommand(modelNumber, upload.SecureUrl, order++));
                }
            }

            return Ok(new { modelNumber, message = "Vehicle model created successfully" });
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
        public async Task<IActionResult> GetVehicleModels(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetVehicleModelsQuery(pageNumber, pageSize);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Search level-2 vehicle models by parent and specs
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchLevel2(
            [FromQuery] string? parentModelNumber = null,
            [FromQuery] int? seats = null,
            [FromQuery] string? fuelType = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await mediator.Send(new SearchLevel2ModelsQuery(parentModelNumber, seats, fuelType, pageNumber, pageSize));
            return Ok(result);
        }
    }
}

