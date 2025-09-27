using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Images.Commands.UploadVehicleImage;
using VehicleShowroomManagement.Application.Features.Images.Commands.DeleteVehicleImage;
using VehicleShowroomManagement.Application.Features.Images.Queries.GetVehicleImages;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for image management operations
    /// </summary>
    [ApiController]
    [Route("api/images")]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Uploads images for a vehicle
        /// </summary>
        [HttpPost("upload/vehicle/{vehicleId}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UploadVehicleImages(string vehicleId, [FromForm] UploadVehicleImagesRequest request)
        {
            if (!request.Images.Any())
                return BadRequest(new { message = "No images provided" });

            var command = new UploadVehicleImageCommand(vehicleId, request.Images);
            var result = await _mediator.Send(command);

            return Ok(new { 
                message = "Images uploaded successfully", 
                uploadedImages = result 
            });
        }

        /// <summary>
        /// Gets all images for a vehicle
        /// </summary>
        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetVehicleImages(string vehicleId)
        {
            var query = new GetVehicleImagesQuery(vehicleId);
            var images = await _mediator.Send(query);

            return Ok(images);
        }

        /// <summary>
        /// Deletes a vehicle image
        /// </summary>
        [HttpDelete("vehicle/{vehicleId}/{imageId}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> DeleteVehicleImage(string vehicleId, string imageId)
        {
            var command = new DeleteVehicleImageCommand(vehicleId, imageId);
            await _mediator.Send(command);

            return Ok(new { message = "Image deleted successfully" });
        }
    }

    /// <summary>
    /// Request model for uploading vehicle images
    /// </summary>
    public class UploadVehicleImagesRequest
    {
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}