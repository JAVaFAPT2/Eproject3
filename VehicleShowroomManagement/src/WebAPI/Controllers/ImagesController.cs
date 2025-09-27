using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Images.Commands;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing vehicle images
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Upload vehicle image to Cloudinary
        /// </summary>
        [HttpPost("upload")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UploadVehicleImage([FromForm] UploadVehicleImageCommand command)
        {
            if (command.ImageFile == null || command.ImageFile.Length == 0)
                return BadRequest(new { message = "No image file provided" });

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(command.ImageFile.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(extension))
                return BadRequest(new { message = "Invalid file type. Only JPG, PNG, GIF, and WebP are allowed." });

            if (command.ImageFile.Length > 10 * 1024 * 1024) // 10MB limit
                return BadRequest(new { message = "File size too large. Maximum 10MB allowed." });

            try
            {
                var imageUrl = await _mediator.Send(command);
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Upload failed: {ex.Message}" });
            }
        }
    }
}
