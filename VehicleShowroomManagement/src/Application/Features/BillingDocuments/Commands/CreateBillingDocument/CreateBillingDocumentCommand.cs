using MediatR;

namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument
{
    public record CreateBillingDocumentCommand(
        string OrderId,
        string CreatedBy,
        decimal Amount,
        DateTime? AppointmentDate = null) : IRequest<string>;
}

