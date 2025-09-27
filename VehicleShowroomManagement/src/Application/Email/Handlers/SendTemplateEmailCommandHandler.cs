using MediatR;
using VehicleShowroomManagement.Application.Email.Commands;
using VehicleShowroomManagement.Application.Email.Services;

namespace VehicleShowroomManagement.Application.Email.Handlers
{
    /// <summary>
    /// Handler for SendTemplateEmailCommand
    /// </summary>
    public class SendTemplateEmailCommandHandler(IEmailService emailService)
        : IRequestHandler<SendTemplateEmailCommand>
    {

        public async Task Handle(SendTemplateEmailCommand request, CancellationToken cancellationToken)
        {
            await emailService.SendTemplateEmailAsync(
                request.TemplateName,
                request.To,
                request.Variables,
                request.Subject);
        }
    }
}
