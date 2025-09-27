using MediatR;
using VehicleShowroomManagement.Application.Email.Commands;
using VehicleShowroomManagement.Application.Email.Models;
using VehicleShowroomManagement.Application.Email.Services;

namespace VehicleShowroomManagement.Application.Email.Handlers
{
    /// <summary>
    /// Handler for SendOrderConfirmationEmailCommand
    /// </summary>
    public class SendOrderConfirmationEmailCommandHandler(IEmailService emailService)
        : IRequestHandler<SendOrderConfirmationEmailCommand>
    {

        public async Task Handle(SendOrderConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            var variables = new Dictionary<string, object>
            {
                { "CustomerName", request.CustomerName },
                { "OrderNumber", request.OrderNumber },
                { "VehicleName", request.VehicleName },
                { "VehicleBrand", request.VehicleBrand },
                { "TotalAmount", request.TotalAmount.ToString("C") },
                { "OrderDate", request.OrderDate.ToString("MMM dd, yyyy") },
                { "Status", request.Status }
            };

            await emailService.SendTemplateEmailAsync(
                EmailTemplateTypes.OrderConfirmation,
                request.CustomerEmail,
                variables);
        }
    }
}
