using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/vehicles/{vehicleId}/photos")]
    [Authorize]
    public class VehiclePhotosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehiclePhotosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all photos for a specific vehicle
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<VehiclePhotoDto>>> GetVehiclePhotos(string vehicleId)
        {
            var query = new GetVehiclePhotosQuery(vehicleId);
            var photos = await _mediator.Send(query);
            return Ok(photos);
        }

        /// <summary>
        /// Gets a specific photo by ID
        /// </summary>
        [HttpGet("~/api/photos/{photoId}")]
        public async Task<ActionResult<VehiclePhotoDto>> GetPhoto(string photoId)
        {
            var query = new GetPhotoByIdQuery(photoId);
            var photo = await _mediator.Send(query);

            if (photo == null)
                return NotFound(new { message = "Photo not found" });

            return Ok(photo);
        }

        /// <summary>
        /// Adds a new photo to a vehicle
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> AddVehiclePhoto(string vehicleId, [FromBody] AddVehiclePhotoRequest request)
        {
            var command = new AddVehiclePhotoCommand(
                vehicleId,
                request.Url,
                request.DisplayOrder,
                request.Caption);

            var photoId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetPhoto), new { photoId }, 
                new { id = photoId, message = "Photo added successfully" });
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

            await _mediator.Send(command);

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
            await _mediator.Send(command);

            return Ok(new { message = "Photo deleted successfully" });
        }
    }
}

