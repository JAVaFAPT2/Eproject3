using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Email.Commands;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,HR")]
    public class EmailController(IMediator mediator) : ControllerBase
    {

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="command">Email command</param>
        /// <returns>Success response</returns>
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailCommand command)
        {
            await mediator.Send(command);
            return Ok(new { message = "Email sent successfully" });
        }

        /// <summary>
        /// Send template email
        /// </summary>
        /// <param name="command">Template email command</param>
        /// <returns>Success response</returns>
        [HttpPost("send-template")]
        public async Task<IActionResult> SendTemplateEmail([FromBody] SendTemplateEmailCommand command)
        {
            await mediator.Send(command);
            return Ok(new { message = "Template email sent successfully" });
        }

        /// <summary>
        /// Send password reset email
        /// </summary>
        /// <param name="command">Password reset email command</param>
        /// <returns>Success response</returns>
        [HttpPost("send-password-reset")]
        public async Task<IActionResult> SendPasswordResetEmail([FromBody] SendPasswordResetEmailCommand command)
        {
            await mediator.Send(command);
            return Ok(new { message = "Password reset email sent successfully" });
        }

        /// <summary>
        /// Send order confirmation email
        /// </summary>
        /// <param name="command">Order confirmation email command</param>
        /// <returns>Success response</returns>
        [HttpPost("send-order-confirmation")]
        public async Task<IActionResult> SendOrderConfirmationEmail([FromBody] SendOrderConfirmationEmailCommand command)
        {
            await mediator.Send(command);
            return Ok(new { message = "Order confirmation email sent successfully" });
        }
    }
}
