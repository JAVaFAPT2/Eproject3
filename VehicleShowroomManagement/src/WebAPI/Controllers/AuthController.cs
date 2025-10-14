using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Auth.Commands.Login;
using VehicleShowroomManagement.Application.Features.Auth.Commands.Register;
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
    public class AuthController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Public user registration (Customer role)
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var command = new RegisterCommand(
                request.Username,
                request.Password,
                request.Email);

            var userId = await mediator.Send(command);

            return CreatedAtAction(nameof(Register), new { id = userId },
                new { id = userId, message = "User registered successfully" });
        }

        /// <summary>
        /// Authenticates user and returns JWT token
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var command = new LoginCommand(request.Username, request.Password);
            var result = await mediator.Send(command);

            if (result == null)
                return Unauthorized(new { message = "Invalid username or password" });

            // Set HttpOnly refresh token cookie
            if (!string.IsNullOrWhiteSpace(result.RefreshToken))
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Path = "/",
                    Expires = result.RefreshTokenExpiresAt
                };
                Response.Cookies.Append("refreshToken", result.RefreshToken, cookieOptions);
            }

            // Optionally omit refresh token from body (kept for backward compatibility)
            return Ok(new
            {
                result.UserId,
                result.RoleName,
                token = result.Token,
                tokenExpiresAt = result.TokenExpiresAt,
                user = result.User
            });
        }

        /// <summary>
        /// Initiates password reset process
        /// </summary>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var command = new ForgotPasswordCommand(request.Email);
            await mediator.Send(command);

            return Ok(new { message = "Password reset instructions have been sent to your email" });
        }

        /// <summary>
        /// Resets password using reset token
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var command = new ResetPasswordCommand(request.Token, request.NewPassword);
            var result = await mediator.Send(command);

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
            // Prefer refresh token from HttpOnly cookie
            var cookieRt = Request.Cookies["refreshToken"];
            var rt = string.IsNullOrWhiteSpace(cookieRt) ? request.RefreshToken : cookieRt;
            var command = new RefreshTokenCommand(rt);
            var result = await mediator.Send(command);

            if (result == null)
                return Unauthorized(new { message = "Invalid refresh token" });

            // Rotate refresh token if provided by handler
            if (!string.IsNullOrWhiteSpace(result.RefreshToken))
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Path = "/",
                    Expires = result.RefreshTokenExpiresAt
                };
                Response.Cookies.Append("refreshToken", result.RefreshToken, cookieOptions);
            }

            return Ok(new
            {
                token = result.Token,
                tokenExpiresAt = result.TokenExpiresAt
            });
        }

        /// <summary>
        /// Revokes refresh token
        /// </summary>
        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            // Prefer cookie value; fallback to body
            var cookieRt = Request.Cookies["refreshToken"];
            var rt = string.IsNullOrWhiteSpace(cookieRt) ? request.RefreshToken : cookieRt;
            var command = new RevokeTokenCommand(rt);
            await mediator.Send(command);

            // Clear HttpOnly cookie
            Response.Cookies.Append("refreshToken", string.Empty, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });

            return Ok(new { message = "Token revoked successfully" });
        }
    }
}