using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VehicleShowroomManagement.Application.Features.Profile.Queries.GetProfile;
using VehicleShowroomManagement.Application.Features.Profile.Commands.UpdateProfile;
using VehicleShowroomManagement.Application.Features.Profile.Commands.ChangePassword;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for user profile management
    /// </summary>
    [ApiController]
    [Route("api/profile")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets current user's profile
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var query = new GetProfileQuery(userId);
            var profile = await _mediator.Send(query);

            if (profile == null)
                return NotFound(new { message = "Profile not found" });

            return Ok(profile);
        }

        /// <summary>
        /// Updates current user's profile
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var command = new UpdateProfileCommand(
                userId,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Phone);

            await _mediator.Send(command);

            return Ok(new { message = "Profile updated successfully" });
        }

        /// <summary>
        /// Changes current user's password
        /// </summary>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var command = new ChangePasswordCommand(
                userId,
                request.CurrentPassword,
                request.NewPassword);

            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest(new { message = "Current password is incorrect" });

            return Ok(new { message = "Password changed successfully" });
        }
    }

    /// <summary>
    /// Request model for updating profile
    /// </summary>
    public class UpdateProfileRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
    }

    /// <summary>
    /// Request model for changing password
    /// </summary>
    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}