using MediatR;
using VehicleShowroomManagement.Application.Email.Commands;
using VehicleShowroomManagement.Application.Email.Models;
using VehicleShowroomManagement.Application.Email.Services;

namespace VehicleShowroomManagement.Application.Email.Handlers
{
    /// <summary>
    /// Handler for SendPasswordResetEmailCommand
    /// </summary>
    public class SendPasswordResetEmailCommandHandler(IEmailService emailService)
        : IRequestHandler<SendPasswordResetEmailCommand>
    {

        public async Task Handle(SendPasswordResetEmailCommand request, CancellationToken cancellationToken)
        {
            var resetUrl = $"{request.BaseUrl}/reset-password?token={request.ResetToken}";

            var variables = new Dictionary<string, object>
            {
                { "Name", request.Name },
                { "ResetUrl", resetUrl },
                { "ExpiryHours", request.ExpiryHours }
            };

            await emailService.SendTemplateEmailAsync(
                EmailTemplateTypes.PasswordReset,
                request.Email,
                variables);
        }
    }
}
