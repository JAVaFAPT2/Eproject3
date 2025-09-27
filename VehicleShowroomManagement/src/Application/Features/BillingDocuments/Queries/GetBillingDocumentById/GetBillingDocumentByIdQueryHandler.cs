using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Queries.GetBillingDocumentById
{
    public class GetBillingDocumentByIdQueryHandler : IRequestHandler<GetBillingDocumentByIdQuery, BillingDocumentDto>
    {
        private readonly IBillingDocumentRepository _billingDocumentRepository;

        public GetBillingDocumentByIdQueryHandler(IBillingDocumentRepository billingDocumentRepository)
        {
            _billingDocumentRepository = billingDocumentRepository;
        }

        public async Task<BillingDocumentDto> Handle(GetBillingDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var billingDocument = await _billingDocumentRepository.GetByIdAsync(request.BillingDocumentId);

            if (billingDocument == null)
                throw new KeyNotFoundException($"Billing document with ID {request.BillingDocumentId} not found");

            return BillingDocumentDto.FromEntity(billingDocument);
        }
    }
}