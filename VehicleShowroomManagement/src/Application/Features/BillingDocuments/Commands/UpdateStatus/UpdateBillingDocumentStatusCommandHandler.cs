namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.UpdateStatus
{
    /// <summary>
    /// Handler for updating billing document status
    /// Uses domain methods: MarkAsPaid, MarkAsUnpaid, MarkAsPartiallyPaid
    /// </summary>
    public class UpdateBillingDocumentStatusCommandHandler(IRepository<BillingDocument> billingDocumentRepository) : IRequestHandler<UpdateBillingDocumentStatusCommand, Unit>
    {

        public async Task<Unit> Handle(UpdateBillingDocumentStatusCommand request, CancellationToken cancellationToken)
        {
            var billingDocument = await billingDocumentRepository.GetByIdAsync(request.BillingDocumentId, cancellationToken) ?? throw new InvalidOperationException("Billing document not found");

            // Use domain methods based on status
            switch (request.Status)
            {
                case BillingStatus.Paid:
                    billingDocument.MarkAsPaid();
                    break;
                case BillingStatus.PartiallyPaid:
                    billingDocument.MarkAsPartiallyPaid();
                    break;
                case BillingStatus.Unpaid:
                    billingDocument.MarkAsUnpaid();
                    break;
                default:
                    throw new ArgumentException($"Invalid billing status: {request.Status}");
            }

            await billingDocumentRepository.UpdateAsync(billingDocument, cancellationToken);

            return Unit.Value;
        }
    }
}

