using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Auth.Commands.Login;
using VehicleShowroomManagement.Application.Features.Auth.Commands.ForgotPassword;
using VehicleShowroomManagement.Application.Features.Auth.Commands.ResetPassword;
using VehicleShowroomManagement.Application.Features.Auth.Commands.RefreshToken;
using VehicleShowroomManagement.Application.Features.Auth.Commands.RevokeToken;
using VehicleShowroomManagement.WebAPI.Models.Auth;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for authentication operations
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Authenticates user and returns JWT token
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var command = new LoginCommand(request.Username, request.Password);
            var result = await _mediator.Send(command);

            if (result == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(result);
        }

        /// <summary>
        /// Initiates password reset process
        /// </summary>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var command = new ForgotPasswordCommand(request.Email);
            await _mediator.Send(command);

            return Ok(new { message = "Password reset instructions have been sent to your email" });
        }

        /// <summary>
        /// Resets password using reset token
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var command = new ResetPasswordCommand(request.Token, request.NewPassword);
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest(new { message = "Invalid or expired reset token" });

            return Ok(new { message = "Password has been reset successfully" });
        }

        /// <summary>
        /// Refreshes JWT token using refresh token
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var command = new RefreshTokenCommand(request.RefreshToken);
            var result = await _mediator.Send(command);

            if (result == null)
                return Unauthorized(new { message = "Invalid refresh token" });

            return Ok(result);
        }

        /// <summary>
        /// Revokes refresh token
        /// </summary>
        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            var command = new RevokeTokenCommand(request.RefreshToken);
            await _mediator.Send(command);

            return Ok(new { message = "Token revoked successfully" });
        }
    }
}