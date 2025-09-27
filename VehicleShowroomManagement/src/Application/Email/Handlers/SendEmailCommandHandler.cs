using MediatR;
using VehicleShowroomManagement.Application.Email.Commands;
using VehicleShowroomManagement.Application.Email.Models;
using VehicleShowroomManagement.Application.Email.Services;

namespace VehicleShowroomManagement.Application.Email.Handlers
{
    /// <summary>
    /// Handler for SendEmailCommand
    /// </summary>
    public class SendEmailCommandHandler(IEmailService emailService)
        : IRequestHandler<SendEmailCommand>
    {

        public async Task Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var emailMessage = new EmailMessage
            {
                To = request.To,
                Cc = request.Cc,
                Bcc = request.Bcc,
                Subject = request.Subject,
                Body = request.Body,
                IsHtml = request.IsHtml,
                Attachments = request.Attachments?.Select(a => new EmailAttachment
                {
                    FileName = a.FileName,
                    Content = a.Content,
                    ContentType = a.ContentType,
                    ContentId = a.ContentId
                }).ToList()
            };

            await emailService.SendEmailAsync(emailMessage);
        }
    }
}
