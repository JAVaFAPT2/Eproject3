using MediatR;

namespace VehicleShowroomManagement.Application.Features.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQuery : IRequest<InvoiceDto>
    {
        public string InvoiceId { get; set; } = string.Empty;
    }
}