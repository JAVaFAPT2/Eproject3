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
        public async Task<IActionResult> CreateVehicleModel(
            [FromForm] string? data,
            [FromForm] string? modelNumber,
            [FromForm] string? name,
            [FromForm] decimal? price,
            [FromForm] string? description,
            [FromForm] string? parentId,
            [FromForm] int? level,
            [FromForm] string? slug,
            [FromForm] List<IFormFile>? files)
        {
            CreateVehicleModelRequest? request = null;

            // Try to parse from JSON data field first
            if (!string.IsNullOrWhiteSpace(data))
            {
                try
                {
                    request = System.Text.Json.JsonSerializer.Deserialize<CreateVehicleModelRequest>(data, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch (System.Text.Json.JsonException)
                {
                    return BadRequest(new { message = "Invalid JSON in 'data' part" });
                }
            }
            // Fallback to individual form fields
            else if (!string.IsNullOrWhiteSpace(name) && price.HasValue)
            {
                // Generate ModelNumber if not provided
                var generatedModelNumber = !string.IsNullOrWhiteSpace(modelNumber) 
                    ? modelNumber 
                    : $"MODEL-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

                request = new CreateVehicleModelRequest
                {
                    ModelNumber = generatedModelNumber,
                    Name = name,
                    Price = price.Value,
                    Description = description ?? string.Empty,
                    ParentId = parentId,
                    Level = level ?? 1,
                    Slug = slug
                };
            }

            if (request is null)
                return BadRequest(new { message = "Missing required fields: data (JSON) or individual fields (name, price). ModelNumber is optional and will be auto-generated if not provided." });

            var command = new CreateVehicleModelCommand(
                request.ModelNumber,
                request.Name,
                request.Price,
                request.Description,
                request.ParentId,
                request.Level,
                request.Slug);

            var createdModelNumber = await mediator.Send(command);

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
                        await mediator.Send(new UpdateVehicleModelCommand(createdModelNumber, request.Name, request.Price, request.Description, request.ParentId, request.Level, request.Slug, upload.SecureUrl));
                    }
                    await mediator.Send(new AddVehiclePhotoCommand(createdModelNumber, upload.SecureUrl, order++));
                }
            }

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

