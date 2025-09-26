using Microsoft.AspNetCore.Mvc;
using MediatR;
using VehicleShowroomManagement.Application.Auth.Commands;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Authentication controller for user login, password reset, and related operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// User login with email and password
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var command = new LoginCommand(request.Email, request.Password);
                var result = await mediator.Send(command);

                return Ok(new
                {
                    token = result.Token,
                    tokenExpiresAt = result.TokenExpiresAt,
                    userId = result.UserId,
                    role = result.RoleName,
                    message = "Login successful"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Request password reset - sends token via email
        /// </summary>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                var command = new ForgotPasswordCommand(request.Email);
                await mediator.Send(command);

                return Ok(new { message = "Password reset token sent to your email" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Reset password using token
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                var command = new ResetPasswordCommand(request.Token, request.NewPassword);
                await mediator.Send(command);

                return Ok(new { message = "Password reset successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    /// <summary>
    /// Request model for user login
    /// </summary>
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for forgot password
    /// </summary>
    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for reset password
    /// </summary>
    public class ResetPasswordRequest
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}