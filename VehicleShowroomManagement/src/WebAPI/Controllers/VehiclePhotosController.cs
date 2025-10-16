using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.AddVehiclePhoto;
using VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.UpdateVehiclePhoto;
using VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.DeleteVehiclePhoto;
using VehicleShowroomManagement.Application.Features.VehiclePhotos.Queries.GetVehiclePhotos;
using VehicleShowroomManagement.Application.Features.VehiclePhotos.Queries.GetPhotoById;
using VehicleShowroomManagement.WebAPI.Models.VehiclePhotos;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for vehicle photo management operations
    /// </summary>
    [ApiController]
    [Route("api/vehicle-models/{modelNumber}/photos")]
    [Authorize]
    public class VehiclePhotosController(IMediator mediator, ICloudinaryService cloudinaryService) : ControllerBase
    {
        /// <summary>
        /// Gets all photos for a specific vehicle model (Level-2)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<VehiclePhotoDto>>> GetVehiclePhotos(string modelNumber)
        {
            var query = new GetVehiclePhotosQuery(modelNumber);
            var photos = await mediator.Send(query);
            return Ok(photos);
        }

        /// <summary>
        /// Gets a specific photo by ID
        /// </summary>
        [HttpGet("~/api/photos/{photoId}")]
        [AllowAnonymous]
        public async Task<ActionResult<VehiclePhotoDto>> GetPhoto(string photoId)
        {
            var query = new GetPhotoByIdQuery(photoId);
            var photo = await mediator.Send(query);

            if (photo == null)
                return NotFound(new { message = "Photo not found" });

            return Ok(photo);
        }

        /// <summary>
        /// Adds a new photo to a vehicle model
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> AddVehiclePhoto(string modelNumber, [FromBody] AddVehiclePhotoRequest request)
        {
            var command = new AddVehiclePhotoCommand(
                modelNumber,
                request.Url,
                request.DisplayOrder,
                request.Caption);

            var photoId = await mediator.Send(command);

            return CreatedAtAction(nameof(GetPhoto), new { photoId }, 
                new { id = photoId, message = "Photo added successfully" });
        }

        /// <summary>
        /// Uploads one or more photo files to a vehicle model. Expects multipart/form-data with repeated part "files".
        /// </summary>
        [HttpPost("upload")]
        [Authorize(Roles = "Dealer,Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadVehiclePhotos(string modelNumber, [FromForm] List<IFormFile> files)
        {
            if (files is not { Count: not 0 })
                return BadRequest(new { message = "No files provided" });

            var created = new List<object>();
            foreach (var file in files)
            {
                if (file is not { Length: not 0 }) continue;
                var upload = await cloudinaryService.UploadImageAsync(file, "vehicles");
                var cmd = new AddVehiclePhotoCommand(modelNumber, upload.SecureUrl);
                var id = await mediator.Send(cmd);
                created.Add(new { id, url = upload.SecureUrl });
            }

            return Ok(new { message = "Photos uploaded successfully", items = created });
        }

        /// <summary>
        /// Updates a vehicle photo
        /// </summary>
        [HttpPut("~/api/photos/{photoId}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateVehiclePhoto(string photoId, [FromBody] UpdateVehiclePhotoRequest request)
        {
            var command = new UpdateVehiclePhotoCommand(
                photoId,
                request.Url,
                request.DisplayOrder,
                request.Caption);

            await mediator.Send(command);

            return Ok(new { message = "Photo updated successfully" });
        }

        /// <summary>
        /// Deletes a vehicle photo
        /// </summary>
        [HttpDelete("~/api/photos/{photoId}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> DeleteVehiclePhoto(string photoId)
        {
            var command = new DeleteVehiclePhotoCommand(photoId);
            await mediator.Send(command);

            return Ok(new { message = "Photo deleted successfully" });
        }
    }
}

