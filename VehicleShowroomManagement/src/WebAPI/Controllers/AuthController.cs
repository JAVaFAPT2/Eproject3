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
                    refreshToken = result.RefreshToken,
                    tokenExpiresAt = result.TokenExpiresAt,
                    refreshTokenExpiresAt = result.RefreshTokenExpiresAt,
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
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var command = new ForgotPasswordCommand 
                { 
                    Email = request.Email,
                    BaseUrl = baseUrl
                };
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
                var command = new ResetPasswordCommand
                {
                    Token = request.Token,
                    NewPassword = request.NewPassword
                };
                await mediator.Send(command);

                return Ok(new { message = "Password reset successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Refresh JWT token using refresh token
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var command = new RefreshTokenCommand
                {
                    RefreshToken = request.RefreshToken,
                    IpAddress = GetIpAddress()
                };
                var result = await mediator.Send(command);

                return Ok(new
                {
                    token = result.Token,
                    refreshToken = result.RefreshToken,
                    tokenExpiresAt = result.TokenExpiresAt,
                    refreshTokenExpiresAt = result.RefreshTokenExpiresAt,
                    userId = result.UserId,
                    role = result.RoleName,
                    message = "Token refreshed successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Revoke refresh token (logout)
        /// </summary>
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            try
            {
                var command = new RevokeTokenCommand
                {
                    RefreshToken = request.RefreshToken,
                    IpAddress = GetIpAddress()
                };
                await mediator.Send(command);

                return Ok(new { message = "Token revoked successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "Unknown";
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

    /// <summary>
    /// Request model for refresh token
    /// </summary>
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for revoke token
    /// </summary>
    public class RevokeTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}