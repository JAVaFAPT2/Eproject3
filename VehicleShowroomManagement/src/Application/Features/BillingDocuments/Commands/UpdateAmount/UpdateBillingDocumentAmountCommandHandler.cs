namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.UpdateAmount
{
    /// <summary>
    /// Handler for updating billing document amount
    /// </summary>
    public class UpdateBillingDocumentAmountCommandHandler : IRequestHandler<UpdateBillingDocumentAmountCommand, Unit>
    {
        private readonly IRepository<BillingDocument> _billingDocumentRepository;

        public UpdateBillingDocumentAmountCommandHandler(IRepository<BillingDocument> billingDocumentRepository)
        {
            _billingDocumentRepository = billingDocumentRepository;
        }

        public async Task<Unit> Handle(UpdateBillingDocumentAmountCommand request, CancellationToken cancellationToken)
        {
            var billingDocument = await _billingDocumentRepository.GetByIdAsync(request.BillingDocumentId);
            if (billingDocument == null)
            {
                throw new InvalidOperationException("Billing document not found");
            }

            // Use domain method
            billingDocument.UpdateAmount(request.Amount);
            await _billingDocumentRepository.UpdateAsync(billingDocument);

            return Unit.Value;
        }
    }
}

