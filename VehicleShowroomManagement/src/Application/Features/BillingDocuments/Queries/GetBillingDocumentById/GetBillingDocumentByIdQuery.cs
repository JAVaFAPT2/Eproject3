using MediatR;

namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Queries.GetBillingDocumentById
{
    public class GetBillingDocumentByIdQuery : IRequest<BillingDocumentDto>
    {
        public string BillingDocumentId { get; set; } = string.Empty;
    }
}