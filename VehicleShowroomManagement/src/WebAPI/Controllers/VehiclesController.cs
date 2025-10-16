using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Features.Vehicles.Commands.CreateVehicle;
using VehicleShowroomManagement.Application.Features.Vehicles.Commands.UpdateVehicle;
using VehicleShowroomManagement.Application.Features.Vehicles.Commands.DeleteVehicle;
using VehicleShowroomManagement.Application.Features.Vehicles.Commands.BulkDeleteVehicles;
using VehicleShowroomManagement.Application.Features.Vehicles.Commands.UpdateVehicleStatus;
using VehicleShowroomManagement.Application.Features.Vehicles.Queries.GetVehicleById;
using VehicleShowroomManagement.Application.Features.Vehicles.Queries.GetVehicles;
using VehicleShowroomManagement.Application.Features.Vehicles.Queries.SearchVehicles;
using VehicleShowroomManagement.WebAPI.Models.Vehicles;
using VehicleShowroomManagement.Domain.Enums;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.AddVehiclePhoto;
using VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModelBySlug;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for vehicle management operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehiclesController(IMediator mediator, ICloudinaryService cloudinaryService) : ControllerBase
    {
        /// <summary>
        /// Creates a new vehicle
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
        {
            // Generate VehicleId if not provided or empty
            var vehicleId = !string.IsNullOrWhiteSpace(request.VehicleId) 
                ? request.VehicleId 
                : $"VEH-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

            var command = new CreateVehicleCommand(
                vehicleId,
                request.ModelNumber,
                request.PurchasePrice,
                request.ExternalNumber,
                request.Vin);

            var createdVehicleId = await mediator.Send(command);
            
            return CreatedAtAction(nameof(GetVehicle), new { id = createdVehicleId }, 
                new { id = createdVehicleId, message = "Vehicle created successfully" });
        }

        /// <summary>
        /// Creates a new vehicle with optional media uploads.
        /// Expects multipart/form-data with part "data" (JSON of CreateVehicleRequest) and optional repeated part "files".
        /// </summary>
        [HttpPost("with-media")]
        [Authorize(Roles = "Dealer,Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateVehicleWithMedia(
            [FromForm] string? data,
            [FromForm] string? vehicleId,
            [FromForm] string? modelNumber,
            [FromForm] decimal? purchasePrice,
            [FromForm] string? externalNumber,
            [FromForm] string? vin,
            [FromForm] List<IFormFile>? files)
        {
            CreateVehicleRequest? request = null;

            // Try to parse from JSON data field first
            if (!string.IsNullOrWhiteSpace(data))
            {
                try
                {
                    request = JsonSerializer.Deserialize<CreateVehicleRequest>(data, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                catch (JsonException)
                {
                    return BadRequest(new { message = "Invalid JSON in 'data' part" });
                }
            }
            // Fallback to individual form fields
            else if (!string.IsNullOrWhiteSpace(vehicleId) && !string.IsNullOrWhiteSpace(modelNumber) && purchasePrice.HasValue)
            {
                request = new CreateVehicleRequest
                {
                    VehicleId = vehicleId,
                    ModelNumber = modelNumber,
                    PurchasePrice = purchasePrice.Value,
                    ExternalNumber = externalNumber,
                    Vin = vin
                };
            }

            if (request is null)
                return BadRequest(new { message = "Missing required fields: data (JSON) or individual fields (vehicleId, modelNumber, purchasePrice)" });

            var createCommand = new CreateVehicleCommand(
                request.VehicleId,
                request.ModelNumber,
                request.PurchasePrice,
                request.ExternalNumber,
                request.Vin);

            var createdVehicleId = await mediator.Send(createCommand);

            if (files is not null && files.Count > 0)
            {
                // Upload each file and create photo records
                foreach (var file in files)
                {
                    if (file is not { Length: not 0 }) continue;
                    var upload = await cloudinaryService.UploadImageAsync(file, "vehicles");
                    var addPhotoCommand = new AddVehiclePhotoCommand(
                        request.ModelNumber,
                        upload.SecureUrl);
                    await mediator.Send(addPhotoCommand);
                }
            }

            return CreatedAtAction(nameof(GetVehicle), new { id = createdVehicleId },
                new { id = createdVehicleId, message = "Vehicle created successfully with media" });
        }

        /// <summary>
        /// Gets a vehicle by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> GetVehicle(string id)
        {
            var query = new GetVehicleByIdQuery(id);
            var vehicle = await mediator.Send(query);

            if (vehicle is null)
                return NotFound(new { message = "Vehicle not found" });

            return Ok(vehicle);
        }

        /// <summary>
        /// Gets vehicles by Level-2 model slug (variant)
        /// </summary>
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetVehiclesByModelSlug(string slug,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var model = await mediator.Send(new GetVehicleModelBySlugQuery(slug));
            if (model is null) return NotFound(new { message = "Vehicle model not found" });

            var result = await mediator.Send(new SearchVehiclesQuery(
                searchTerm: null,
                status: null,
                modelNumber: model.ModelNumber,
                seats: null,
                fuelType: null,
                minPrice: null,
                maxPrice: null,
                pageNumber: pageNumber,
                pageSize: pageSize));

            return Ok(new { model, vehicles = result });
        }

        /// <summary>
        /// Gets vehicles with optional search and filters (merged with previous /search)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<SearchVehiclesResult>> GetVehicles(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] VehicleStatus? status = null,
            [FromQuery] string? modelNumber = null,
            [FromQuery] int? seats = null,
            [FromQuery] string? fuelType = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            var query = new SearchVehiclesQuery(
                searchTerm,
                status,
                modelNumber,
                seats,
                fuelType,
                minPrice,
                maxPrice,
                pageNumber,
                pageSize);

            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Deprecated: use GET /api/vehicles with query params instead
        /// </summary>
        [HttpGet("search")]
        [Obsolete]
        public IActionResult SearchVehicles()
        {
            return BadRequest(new { message = "Use GET /api/vehicles with query params" });
        }

        /// <summary>
        /// Updates a vehicle
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateVehicle(string id, [FromBody] UpdateVehicleRequest request)
        {
            var command = new UpdateVehicleCommand(
                id,
                request.ModelNumber,
                request.PurchasePrice,
                request.ExternalNumber,
                request.Vin,
                request.LicensePlate,
                request.Color,
                request.Mileage);

            await mediator.Send(command);
            
            return Ok(new { message = "Vehicle updated successfully" });
        }

        /// <summary>
        /// Deletes a vehicle
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVehicle(string id)
        {
            var command = new DeleteVehicleCommand(id);
            await mediator.Send(command);
            
            return Ok(new { message = "Vehicle deleted successfully" });
        }

        /// <summary>
        /// Bulk delete vehicles
        /// </summary>
        [HttpPost("bulk-delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BulkDeleteVehicles([FromBody] BulkDeleteVehiclesRequest request)
        {
            var command = new BulkDeleteVehiclesCommand(request.VehicleIds);
            await mediator.Send(command);
            
            return Ok(new { message = $"{request.VehicleIds.Count} vehicles deleted successfully" });
        }

        /// <summary>
        /// Updates vehicle status - for inventory management
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateVehicleStatus(string id, [FromBody] UpdateVehicleStatusRequest request)
        {
            var command = new UpdateVehicleStatusCommand(id, request.Status);
            await mediator.Send(command);
            
            return Ok(new { message = "Vehicle status updated successfully" });
        }
    }
}