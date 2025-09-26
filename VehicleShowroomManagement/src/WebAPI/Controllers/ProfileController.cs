using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Application.Commands;
using VehicleShowroomManagement.Application.Queries;
using System.Threading.Tasks;
using System.Security.Claims;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Profile controller for user profile management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get current user profile
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var query = new GetUserProfileQuery(userId);
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var command = new UpdateProfileCommand(
                    userId,
                    request.FullName,
                    request.Address,
                    request.Phone);

                await _mediator.Send(command);

                return Ok(new { message = "Profile updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Change password
        /// </summary>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var command = new ChangePasswordCommand(
                    userId,
                    request.OldPassword,
                    request.NewPassword);

                await _mediator.Send(command);

                return Ok(new { message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    /// <summary>
    /// Request model for updating profile
    /// </summary>
    public class UpdateProfileRequest
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }

    /// <summary>
    /// Request model for changing password
    /// </summary>
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}